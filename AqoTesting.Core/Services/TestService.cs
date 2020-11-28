﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AqoTesting.Core.Utils;
using AqoTesting.Shared.DTOs.API.Common.Identifiers;
using AqoTesting.Shared.DTOs.API.MemberAPI.Tests;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections;
using AqoTesting.Shared.DTOs.DB.Attempts;
using AqoTesting.Shared.DTOs.DB.Tests;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AutoMapper;
using MongoDB.Bson;

namespace AqoTesting.Core.Services
{
    public class TestService : ServiceBase, ITestService
    {
        ITestRepository _testRepository;
        IRoomRepository _roomRepository;
        IAttemptRepository _attemptRepository;
        IWorkContext _workContext;

        public TestService(ITestRepository testRepository, IRoomRepository roomRespository, IAttemptRepository attemptRepository, IWorkContext workContext)
        {
            _testRepository = testRepository;
            _roomRepository = roomRespository;
            _attemptRepository = attemptRepository;
            _workContext = workContext;
        }

        #region UserAPI
        public async Task<(OperationErrorMessages, object)> UserAPI_GetTestsByRoomId(ObjectId roomId)
        {
            var tests = await _testRepository.GetTestsByRoomId(roomId);
            var getTestsItemDTOs = Mapper.Map<UserAPI_GetTestsItem_DTO[]>(tests);

            return (OperationErrorMessages.NoError, getTestsItemDTOs);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_GetTestsByRoomId(CommonAPI_RoomId_DTO roomIdDTO) =>
            await UserAPI_GetTestsByRoomId(ObjectId.Parse(roomIdDTO.RoomId));

        public async Task<(OperationErrorMessages, object)> UserAPI_GetTestById(ObjectId testId)
        {
            var test = await _testRepository.GetTestById(testId);
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

            if (outdatedTest.Sections.Count > 0 && postTestDTO.AttemptSectionsNumber > outdatedTest.Sections.Count)
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

        public async Task<(OperationErrorMessages, object)> UserAPI_EditSections(ObjectId testId, UserAPI_PostTestSections_DTO postSectionsDTO)
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

            if (dbSections.Count > 0 && dbSections.Count < test.AttemptSectionsNumber)
                return (OperationErrorMessages.NotEnoughSections, null);

            await _testRepository.SetProperty(testId, "Sections", dbSections);

            return (OperationErrorMessages.NoError, null);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_EditSections(CommonAPI_TestId_DTO testIdDTO, UserAPI_PostTestSections_DTO postSectionsDTO) =>
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
            await this.MemberAPI_GetTestsByRoomId(ObjectId.Parse(roomIdDTO.RoomId));

        public async Task<(OperationErrorMessages, object)> MemberAPI_GetTestById(ObjectId testId)
        {
            var test = await _testRepository.GetTestById(testId);

            if (test == null)
                return (OperationErrorMessages.TestNotFound, null);

            var getTestDTO = Mapper.Map<MemberAPI_GetTest_DTO>(test);

            return (OperationErrorMessages.NoError, getTestDTO);
        }
        public async Task<(OperationErrorMessages, object)> MemberAPI_GetTestById(CommonAPI_TestId_DTO testIdDTO) =>
            await this.MemberAPI_GetTestById(ObjectId.Parse(testIdDTO.TestId));

        public async Task<(OperationErrorMessages, object)> MemberAPI_BeginTest(ObjectId testId)
        {
            var memberId = _workContext.MemberId;

            var test = await _testRepository.GetTestById(testId);
            if (!test.IsActive && (test.DeactivationDate == null || test.DeactivationDate < DateTime.Now || (test.ActivationDate != null && test.ActivationDate.Value > DateTime.Now)))
                return (OperationErrorMessages.TestIsNotActive, null);

            var attempts = (await _attemptRepository.GetAttemptsByTestIdAndMemberId(testId, memberId));

            if (attempts.Count() >= test.AttemptsNumber)
                return (OperationErrorMessages.NoAttemptsLeft, null);

            var newAttempt = Mapper.Map<AttemptsDB_Attempt_DTO>(test);
            newAttempt.StartDate = DateTime.Now;
            newAttempt.EndDate = newAttempt.StartDate.Value.AddMinutes(test.TimeLimit);
            newAttempt.MemberId = memberId;

            var newAttemptId = await _attemptRepository.InsertAttempt(newAttempt);

            return (OperationErrorMessages.NoError, new CommonAPI_AttemptId_DTO { AttemptId = newAttemptId.ToString() });
        }
        public async Task<(OperationErrorMessages, object)> MemberAPI_BeginTest(CommonAPI_TestId_DTO testIdDTO) =>
            await this.MemberAPI_BeginTest(ObjectId.Parse(testIdDTO.TestId));
        #endregion
    }
}