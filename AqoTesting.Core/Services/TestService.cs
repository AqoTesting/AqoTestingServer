using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AqoTesting.Core.Utils;
using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.DTOs.API.MemberAPI.Tests;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests;
using AqoTesting.Shared.DTOs.DB.Attempts;
using AqoTesting.Shared.DTOs.DB.Tests;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
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

        public async Task<(OperationErrorMessages, object)> UserAPI_CreateTest(CommonAPI_RoomId_DTO roomIdDTO, UserAPI_PostTest_DTO postTestDTO)
        {
            var (valid, errorCode, response) = SectionsValidator.Validate(postTestDTO.Sections);

            if (!valid)
                return (errorCode, response);

            /*var newTest = Mapper.Map<TestsDB_Test_DTO>(postTestDTO);
            newTest.OwnerId = _workContext.UserId;
            newTest.RoomId = ObjectId.Parse(roomIdDTO.RoomId);
            newTest.CreationDate = DateTime.UtcNow;

            return (OperationErrorMessages.NoError, newTest);*/
            return (OperationErrorMessages.NoError, "OK");
        }
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