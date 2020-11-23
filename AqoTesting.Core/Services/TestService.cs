using System;
using System.Linq;
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
            newTest.OwnerId = _workContext.UserId;
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

            var updatedTest = Mapper.Map<TestsDB_Test_DTO>(postTestDTO);
            updatedTest.OwnerId = outdatedTest.OwnerId;
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
            var (valid, errorCode, response) = SectionsValidator.Validate(postSectionsDTO.Sections);

            var test = await _testRepository.GetTestById(testId);
            if (test == null)
                return (OperationErrorMessages.TestNotFound, null);

            var dbSections = test.Sections;

            foreach(var updateSection in postSectionsDTO.Sections)
            {
                if (updateSection.Value.Deleted)
                    if (!dbSections.ContainsKey(updateSection.Key))
                        return (OperationErrorMessages.SectionNotFound, new CommonAPI_Error_DTO { ErrorSubject = updateSection.Key });
                    else
                        dbSections.Remove(updateSection.Key);

                else
                {
                    foreach (var updateQuestion in updateSection.Value.Questions)
                        if (updateQuestion.Value.Deleted)
                            if (!dbSections.ContainsKey(updateSection.Key))
                                return (OperationErrorMessages.SectionNotFound, new CommonAPI_Error_DTO { ErrorSubject = updateSection.Key });
                            else if (!dbSections[updateSection.Key].Questions.ContainsKey(updateQuestion.Key))
                                return (OperationErrorMessages.QuestionNotFound, new CommonAPI_Error_DTO { ErrorSubject = new string[] { updateSection.Key, updateQuestion.Key } });

                            else
                            {
                                dbSections[updateSection.Key].Questions.Remove(updateQuestion.Key);
                                updateSection.Value.Questions.Remove(updateQuestion.Key);
                            }

                    if (updateSection.Value.Questions.Count > 0)
                        if (dbSections.ContainsKey(updateSection.Key))
                        {
                            var oldQuestions = dbSections[updateSection.Key].Questions.ToDictionary(x => x.Key, x => x.Value);
                            dbSections[updateSection.Key] = Mapper.Map<TestsDB_Section_DTO>(updateSection.Value);
                            dbSections[updateSection.Key].Questions.Concat(oldQuestions);
                        }
                        else
                            dbSections.Add(updateSection.Key, Mapper.Map<TestsDB_Section_DTO>(updateSection.Value));
                }
                
            }

            await _testRepository.SetSections(testId, dbSections);

            return (OperationErrorMessages.NoError, null);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_EditSections(CommonAPI_TestId_DTO testIdDTO, UserAPI_PostSections_DTO postSectionsDTO) =>
            await this.UserAPI_EditSections(ObjectId.Parse(testIdDTO.TestId), postSectionsDTO);
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