﻿using AqoTesting.Core.Utils;
using AqoTesting.Shared.DTOs.API.Common.Identifiers;
using AqoTesting.Shared.DTOs.API.MemberAPI.Attempts;
using AqoTesting.Shared.DTOs.API.UserAPI.Attempts;
using AqoTesting.Shared.DTOs.DB.Attempts;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AutoMapper;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AqoTesting.Core.Services
{
    public class AttemptService : ServiceBase, IAttemptService
    {
        IAttemptRepository _attemptRepository;
        IMemberRepository _memberRepository;
        ICacheRepository _cacheRepository;
        IWorkContext _workContext;

        public AttemptService(IAttemptRepository attemptRepository, IMemberRepository memberRepository, IWorkContext workContext)
        {
            _attemptRepository = attemptRepository;
            _memberRepository = memberRepository;
            _workContext = workContext;
        }

        #region UserAPI
        public async Task<(OperationErrorMessages, object)> UserAPI_GetAttempt(ObjectId attemptId)
        {
            var attempt = await _attemptRepository.GetAttemptById(attemptId);
            var getAttemptDTO = Mapper.Map<UserAPI_GetAttempt_DTO>(attempt);

            return (OperationErrorMessages.NoError, getAttemptDTO);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_GetAttempt(CommonAPI_AttemptId_DTO attemptIdDTO) =>
            await this.UserAPI_GetAttempt(ObjectId.Parse(attemptIdDTO.AttemptId));

        public async Task<(OperationErrorMessages, object)> UserAPI_GetAttemptsByTestId(ObjectId testId)
        {
            var attempts = await _attemptRepository.GetAttemptsByTestId(testId);
            var getAttemptDTO = Mapper.Map<UserAPI_GetAttemptsItem_DTO[]>(attempts);

            return (OperationErrorMessages.NoError, getAttemptDTO);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_GetAttemptsByTestId(CommonAPI_TestId_DTO testIdDTO) =>
            await this.UserAPI_GetAttemptsByTestId(ObjectId.Parse(testIdDTO.TestId));

        public async Task<(OperationErrorMessages, object)> UserAPI_GetAttemptsByMemberId(ObjectId memberId)
        {
            var attempts = await _attemptRepository.GetAttemptsByMemberId(memberId);
            var getAttemptDTO = Mapper.Map<UserAPI_GetAttemptsItem_DTO[]>(attempts);

            return (OperationErrorMessages.NoError, getAttemptDTO);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_GetAttemptsByMemberId(CommonAPI_MemberId_DTO memberIdDTO) =>
            await this.UserAPI_GetAttemptsByMemberId(ObjectId.Parse(memberIdDTO.MemberId));
        #endregion

        #region MemberAPI
        public async Task<(OperationErrorMessages, object)> MemberAPI_GetActiveAttempt()
        {
            var memberId = _workContext.MemberId.Value;
            var attempt = await _attemptRepository.GetActiveAttemptByMemberId(memberId);
            if (attempt == null)
                return (OperationErrorMessages.HasNoActiveAttempt, null);
            var getAttemptDTO = Mapper.Map<MemberAPI_GetAttempt_DTO>(attempt);

            return (OperationErrorMessages.NoError, getAttemptDTO);
        }

        public async Task<(OperationErrorMessages, object)> MemberAPI_Answer(CommonAPI_TestSectionId_DTO sectionIdDTO, CommonAPI_TestQuestionId_DTO questionIdDTO, MemberAPI_CommonTestAnswer_DTO commonAnswerDTO)
        {
            var memberId = _workContext.MemberId.Value;

            var sectionId = sectionIdDTO.SectionId;
            var questionId = questionIdDTO.QuestionId;

            var attempt = await _attemptRepository.GetActiveAttemptByMemberId(memberId);

            var (valid, errorCode, response) = AttemptUtils.ApplyAnswer(attempt.Sections, sectionId, questionId, commonAnswerDTO);
            if (!valid)
                return (errorCode, response);

            await _attemptRepository.SetProperties(attempt.Id, new Dictionary<string, object> {
                ["Sections"] = response,
                ["CurrentSectionId"] = sectionId,
                ["CurrentQuestionId"] = questionId
            });

            return (OperationErrorMessages.NoError, null);
        }

        public async Task<(OperationErrorMessages, object)> CommonAPI_FinishAttempt(AttemptsDB_Attempt_DTO attempt)
        {
            var propertiesToUpdate = new Dictionary<string, object> {
                ["IsActive"] = false };

            var timeNow = DateTime.Now;
            if (timeNow <= attempt.EndDate)
                propertiesToUpdate.Add("EndDate", timeNow);

            await _attemptRepository.SetProperties(attempt.Id, propertiesToUpdate);

            return (OperationErrorMessages.NoError, null);
        }
        public async Task<(OperationErrorMessages, object)> MemberAPI_FinishAttemptByMemberId(ObjectId memberId) =>
            await this.CommonAPI_FinishAttempt(
                await _attemptRepository.GetActiveAttemptByMemberId(memberId));
        public async Task<(OperationErrorMessages, object)> MemberAPI_FinishAttemptByMemberId(CommonAPI_MemberId_DTO memberIdDTO) =>
            await this.MemberAPI_FinishAttemptByMemberId(ObjectId.Parse(memberIdDTO.MemberId));
        public async Task<(OperationErrorMessages, object)> MemberAPI_FinishAttemptById(ObjectId attemptId) =>
            await this.CommonAPI_FinishAttempt(
                await _attemptRepository.GetAttemptById(attemptId));
        public async Task<(OperationErrorMessages, object)> MemberAPI_FinishAttemptById(CommonAPI_AttemptId_DTO attemptIdDTO) =>
            await this.MemberAPI_FinishAttemptById(ObjectId.Parse(attemptIdDTO.AttemptId));
        #endregion
    }
}