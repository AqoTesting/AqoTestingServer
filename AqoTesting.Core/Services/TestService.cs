using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AqoTesting.Core.Utils;
using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.DTOs.API.MemberAPI.Tests;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections;
using AqoTesting.Shared.DTOs.DB.Tests;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AutoMapper;
using MongoDB.Bson;

namespace AqoTesting.Core.Services
{
    public class TestService : ServiceBase, ITestService
    {
        IRoomRepository _roomRepository;
        ITestRepository _testRepository;
        IWorkContext _workContext;

        public TestService(IRoomRepository roomRespository, ITestRepository testRepository, IWorkContext workContext)
        {
            _roomRepository = roomRespository;
            _testRepository = testRepository;
            _workContext = workContext;
        }

        #region UserAPI
        public async Task<(OperationErrorMessages, object)> UserAPI_GetTestsByRoomId(ObjectId roomId)
        {
            var room = await _roomRepository.GetRoomById(roomId);

            if(room == null)
                return (OperationErrorMessages.RoomNotFound, null);

            var tests = await _testRepository.GetTestsByRoomId(room.Id);

            var getTestsItemDTOs = Mapper.Map<UserAPI_GetTestsItem_DTO[]>(tests);

            return (OperationErrorMessages.NoError, getTestsItemDTOs);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_GetTestsByRoomId(CommonAPI_RoomId_DTO roomIdDTO) =>
            await UserAPI_GetTestsByRoomId(ObjectId.Parse(roomIdDTO.RoomId));

        public async Task<(OperationErrorMessages, object)> UserAPI_GetTestById(ObjectId testId)
        {
            var test = await _testRepository.GetTestById(testId);

            if(test == null)
                return (OperationErrorMessages.TestNotFound, null);

            var getTestDTO = Mapper.Map<UserAPI_GetTest_DTO>(test);

            return (OperationErrorMessages.NoError, getTestDTO);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_GetTestById(CommonAPI_TestId_DTO testIdDTO) =>
            await UserAPI_GetTestById(ObjectId.Parse(testIdDTO.TestId));

        public async Task<(OperationErrorMessages, object)> UserAPI_CreateTest(ObjectId roomId, UserAPI_PostTest_DTO postTestDTO)
        {
            var newTest = Mapper.Map<TestsDB_Test_DTO>(postTestDTO);
            newTest.UserId = _workContext.UserId;
            newTest.RoomId = roomId;
            newTest.CreationDate = DateTime.UtcNow;

            var newTestId = await _testRepository.InsertTest(newTest);
            var newTestIdDTO = new CommonAPI_TestId_DTO { TestId = newTestId.ToString() };

            return (OperationErrorMessages.NoError, newTestIdDTO);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_CreateTest(CommonAPI_RoomId_DTO roomIdDTO, UserAPI_PostTest_DTO postTestDTO) =>
            await this.UserAPI_CreateTest(ObjectId.Parse(roomIdDTO.RoomId), postTestDTO);

        public async Task<(OperationErrorMessages, object)> UserAPI_EditTest(ObjectId testId, UserAPI_PostTest_DTO postTestDTO)
        {
            var outdatedTest = await _testRepository.GetTestById(testId);
            if (outdatedTest == null)
                return (OperationErrorMessages.TestNotFound, null);

            if (postTestDTO.AttemptSectionsNumber > outdatedTest.Sections.Count)
                return (OperationErrorMessages.NotEnoughSections, null);

            var updatedTest = Mapper.Map<TestsDB_Test_DTO>(postTestDTO);
            updatedTest.Id = outdatedTest.Id;
            updatedTest.UserId = outdatedTest.UserId;
            updatedTest.RoomId = outdatedTest.RoomId;
            updatedTest.CreationDate = outdatedTest.CreationDate;
            updatedTest.Sections = outdatedTest.Sections;

            await _testRepository.ReplaceTest(updatedTest);

            return (OperationErrorMessages.NoError, null);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_EditTest(CommonAPI_TestId_DTO testIdDTO, UserAPI_PostTest_DTO postTestDTO) =>
            await this.UserAPI_EditTest(ObjectId.Parse(testIdDTO.TestId), postTestDTO);

        public async Task<(OperationErrorMessages, object)> UserAPI_EditSections(ObjectId testId, UserAPI_PostSections_DTO postSectionsDTO)
        {
            var (valid, errorCode, response) = TestsUtils.ValidateSections(postSectionsDTO.Sections);
            if (!valid)
                return (errorCode, response);

            var test = await _testRepository.GetTestById(testId);
            if (test == null)
                return (OperationErrorMessages.TestNotFound, null);

            bool merged;
            (merged, errorCode, response) = TestsUtils.MergeSections(test.Sections, postSectionsDTO.Sections);
            if (!merged)
                return (errorCode, response);

            var dbSections = (Dictionary<string, TestsDB_Section_DTO>) response;

            if (dbSections.Count < test.AttemptSectionsNumber)
                return (OperationErrorMessages.NotEnoughSections, null);

            await _testRepository.SetSections(testId, dbSections);

            return (OperationErrorMessages.NoError, null);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_EditSections(CommonAPI_TestId_DTO testIdDTO, UserAPI_PostSections_DTO postSectionsDTO) =>
            await this.UserAPI_EditSections(ObjectId.Parse(testIdDTO.TestId), postSectionsDTO);

        public async Task<(OperationErrorMessages, object)> UserAPI_DeleteTest(ObjectId testId)
        {
            var deleted = await _testRepository.DeleteTest(testId);
            if (!deleted)
                return (OperationErrorMessages.TestNotFound, null);

            return (OperationErrorMessages.NoError, null);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_DeleteTest(CommonAPI_TestId_DTO testIdDTO) =>
            await this.UserAPI_DeleteTest(ObjectId.Parse(testIdDTO.TestId));
        #endregion

        #region MemberAPI
        public async Task<(OperationErrorMessages, object)> MemberAPI_GetTestsByRoomId(ObjectId roomId)
        {
            var room = await _roomRepository.GetRoomById(roomId);

            if (room == null)
                return (OperationErrorMessages.RoomNotFound, null);

            var tests = await _testRepository.GetTestsByRoomId(room.Id);

            var getTestsItemDTOs = Mapper.Map<MemberAPI_GetTestsItem_DTO[]>(tests);

            return (OperationErrorMessages.NoError, getTestsItemDTOs);
        }
        public async Task<(OperationErrorMessages, object)> MemberAPI_GetTestsByRoomId(CommonAPI_RoomId_DTO roomIdDTO) =>
            await MemberAPI_GetTestsByRoomId(ObjectId.Parse(roomIdDTO.RoomId));

        public async Task<(OperationErrorMessages, object)> MemberAPI_GetTestById(ObjectId testId)
        {
            var test = await _testRepository.GetTestById(testId);

            if (test == null)
                return (OperationErrorMessages.TestNotFound, null);

            var getTestDTO = Mapper.Map<MemberAPI_GetTest_DTO>(test);

            return (OperationErrorMessages.NoError, getTestDTO);
        }
        public async Task<(OperationErrorMessages, object)> MemberAPI_GetTestById(CommonAPI_TestId_DTO testIdDTO) =>
            await MemberAPI_GetTestById(ObjectId.Parse(testIdDTO.TestId));
        #endregion
    }
}