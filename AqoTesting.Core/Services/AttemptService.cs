using AqoTesting.Core.Utils;
using AqoTesting.Shared.DTOs.API.CommonAPI;
using AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers;
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
        IWorkContext _workContext;

        public AttemptService(IAttemptRepository attemptRepository, IWorkContext workContext)
        {
            _attemptRepository = attemptRepository;
            _workContext = workContext;
        }

        #region UserAPI
        public async Task<(OperationErrorMessages, object)> UserAPI_GetAttempt(ObjectId attemptId)
        {
            var attempt = await _attemptRepository.GetAttemptById(attemptId);
            var getAttemptDTO = Mapper.Map<UserAPI_GetAttemptDTO>(attempt);

            return (OperationErrorMessages.NoError, getAttemptDTO);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_GetAttempt(CommonAPI_AttemptIdDTO attemptIdDTO) =>
            await this.UserAPI_GetAttempt(ObjectId.Parse(attemptIdDTO.AttemptId));

        public async Task<(OperationErrorMessages, object)> UserAPI_GetAttemptsByTestId(ObjectId testId)
        {
            var attempts = await _attemptRepository.GetAttemptsByTestId(testId);
            var getAttemptDTO = Mapper.Map<UserAPI_GetAttemptsItemDTO[]>(attempts);

            return (OperationErrorMessages.NoError, getAttemptDTO);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_GetAttemptsByTestId(CommonAPI_TestIdDTO testIdDTO) =>
            await this.UserAPI_GetAttemptsByTestId(ObjectId.Parse(testIdDTO.TestId));

        public async Task<(OperationErrorMessages, object)> UserAPI_GetAttemptsByMemberId(ObjectId memberId)
        {
            var attempts = await _attemptRepository.GetAttemptsByMemberId(memberId);
            var getAttemptDTO = Mapper.Map<UserAPI_GetAttemptsItemDTO[]>(attempts);

            return (OperationErrorMessages.NoError, getAttemptDTO);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_GetAttemptsByMemberId(CommonAPI_MemberIdDTO memberIdDTO) =>
            await this.UserAPI_GetAttemptsByMemberId(ObjectId.Parse(memberIdDTO.MemberId));

        public async Task<(OperationErrorMessages, object)> UserAPI_SetAttemptIgnore(ObjectId attemptId, bool newValue)
        {
            await _attemptRepository.SetProperty(attemptId, "Ignore", newValue);

            return (OperationErrorMessages.NoError, null);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_SetAttemptIgnore(CommonAPI_AttemptIdDTO attemptIdDTO, bool newValue) =>
            await this.UserAPI_SetAttemptIgnore(ObjectId.Parse(attemptIdDTO.AttemptId), newValue);
        public async Task<(OperationErrorMessages, object)> UserAPI_SetAttemptIgnore(ObjectId attemptId, CommonAPI_BooleanValueDTO booleanValueDTO) =>
            await this.UserAPI_SetAttemptIgnore(attemptId, booleanValueDTO.BooleanValue.Value);
        public async Task<(OperationErrorMessages, object)> UserAPI_SetAttemptIgnore(CommonAPI_AttemptIdDTO attemptIdDTO, CommonAPI_BooleanValueDTO booleanValueDTO) =>
            await this.UserAPI_SetAttemptIgnore(ObjectId.Parse(attemptIdDTO.AttemptId), booleanValueDTO.BooleanValue.Value);

        public async Task<(OperationErrorMessages, object)> UserAPI_DeleteAttempt(ObjectId attemptId)
        {
            var deleted = await _attemptRepository.Delete(attemptId);

            if(!deleted)
                return (OperationErrorMessages.AttemptNotFound, null);

            return (OperationErrorMessages.NoError, null);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_DeleteAttempt(CommonAPI_AttemptIdDTO attemptIdDTO) =>
            await this.UserAPI_DeleteAttempt(ObjectId.Parse(attemptIdDTO.AttemptId));
        #endregion

        #region MemberAPI
        public async Task<(OperationErrorMessages, object)> MemberAPI_GetAttempt(ObjectId attemptId)
        {
            var attempt = await _attemptRepository.GetAttemptById(attemptId);
            var getAttemptDTO = Mapper.Map<MemberAPI_GetAttemptDTO>(attempt);

            return (OperationErrorMessages.NoError, getAttemptDTO);
        }
        public async Task<(OperationErrorMessages, object)> MemberAPI_GetAttempt(CommonAPI_AttemptIdDTO attemptIdDTO) =>
            await this.MemberAPI_GetAttempt(ObjectId.Parse(attemptIdDTO.AttemptId));

        public async Task<(OperationErrorMessages, object)> MemberAPI_GetActiveAttempt()
        {
            var memberId = _workContext.MemberId.Value;
            var attempt = await _attemptRepository.GetActiveAttemptByMemberId(memberId);
            var getAttemptDTO = Mapper.Map<MemberAPI_GetAttemptDTO>(attempt);

            return (OperationErrorMessages.NoError, getAttemptDTO);
        }
        public async Task<(OperationErrorMessages, object)> MemberAPI_GetActiveAttemptResumeInfo()
        {
            var memberId = _workContext.MemberId.Value;
            var attempt = await _attemptRepository.GetActiveAttemptByMemberId(memberId);
            var resumeInfoDTO = Mapper.Map<MemberAPI_ActiveAttemptResumeDataDTO>(attempt);

            return (OperationErrorMessages.NoError, resumeInfoDTO);
        }

        public async Task<(OperationErrorMessages, object)> MemberAPI_GetAttemptsByMemberId(ObjectId memberId)
        {
            var attempts = await _attemptRepository.GetAttemptsByMemberId(memberId);
            var getAttemptItemDTOs = Mapper.Map<MemberAPI_GetAttemptsItemDTO[]>(attempts);

            return (OperationErrorMessages.NoError, getAttemptItemDTOs);
        }
        public async Task<(OperationErrorMessages, object)> MemberAPI_GetAttemptsByMemberId(CommonAPI_MemberIdDTO memberIdDTO) =>
            await this.MemberAPI_GetAttemptsByMemberId(ObjectId.Parse(memberIdDTO.MemberId));

        public async Task<(OperationErrorMessages, object)> GetAttemptsByTestIdAndMemberId(ObjectId testId, ObjectId memberId)
        {
            var attempts = await _attemptRepository.GetAttemptsByTestIdAndMemberId(testId, memberId);
            var getAttemptItemDTOs = Mapper.Map<MemberAPI_GetAttemptsItemDTO[]>(attempts);

            return (OperationErrorMessages.NoError, getAttemptItemDTOs);
        }
        public async Task<(OperationErrorMessages, object)> GetAttemptsByTestIdAndMemberId(CommonAPI_TestIdDTO testIdDTO, ObjectId memberId) =>
            await this.GetAttemptsByTestIdAndMemberId(ObjectId.Parse(testIdDTO.TestId), memberId);
        public async Task<(OperationErrorMessages, object)> GetAttemptsByTestIdAndMemberId(ObjectId testId, CommonAPI_MemberIdDTO memberIdDTO) =>
            await this.GetAttemptsByTestIdAndMemberId(testId, ObjectId.Parse(memberIdDTO.MemberId));
        public async Task<(OperationErrorMessages, object)> GetAttemptsByTestIdAndMemberId(CommonAPI_TestIdDTO testIdDTO, CommonAPI_MemberIdDTO memberIdDTO) =>
            await this.GetAttemptsByTestIdAndMemberId(ObjectId.Parse(testIdDTO.TestId), ObjectId.Parse(memberIdDTO.MemberId));

        public async Task<(OperationErrorMessages, object)> MemberAPI_Answer(CommonAPI_TestSectionIdDTO sectionIdDTO, CommonAPI_TestQuestionIdDTO questionIdDTO, MemberAPI_CommonTestAnswerDTO commonAnswerDTO)
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

        public async Task<(OperationErrorMessages, object)> MemberAPI_FinishAttemptByMemberId(ObjectId memberId) =>
            await this.CommonAPI_FinishAttempt(
                await _attemptRepository.GetActiveAttemptByMemberId(memberId));
        public async Task<(OperationErrorMessages, object)> MemberAPI_FinishAttemptByMemberId(CommonAPI_MemberIdDTO memberIdDTO) =>
            await this.MemberAPI_FinishAttemptByMemberId(ObjectId.Parse(memberIdDTO.MemberId));
        public async Task<(OperationErrorMessages, object)> MemberAPI_FinishAttemptById(ObjectId attemptId) =>
            await this.CommonAPI_FinishAttempt(
                await _attemptRepository.GetAttemptById(attemptId));
        public async Task<(OperationErrorMessages, object)> MemberAPI_FinishAttemptById(CommonAPI_AttemptIdDTO attemptIdDTO) =>
            await this.MemberAPI_FinishAttemptById(ObjectId.Parse(attemptIdDTO.AttemptId));
        #endregion

        #region CommonAPI
        public async Task<(OperationErrorMessages, object)> CommonAPI_FinishAttempt(AttemptsDB_AttemptDTO attempt)
        {
            var propertiesToUpdate = new Dictionary<string, object>
            {
                ["IsActive"] = false
            };

            var timeNow = DateTime.Now;
            if(attempt.StartDate == attempt.EndDate || timeNow <= attempt.EndDate)
                propertiesToUpdate.Add("EndDate", timeNow);

            var (totalBlurTime, maxPoints, correctPoints, penalPoints, correctRatio, penalRatio) = AttemptUtils.CalculateScore(attempt.Sections);
            propertiesToUpdate.Add("TotalBlurTime", totalBlurTime);
            propertiesToUpdate.Add("MaxPoints", maxPoints);
            propertiesToUpdate.Add("CorrectPoints", correctPoints);
            propertiesToUpdate.Add("PenalPoints", penalPoints);
            propertiesToUpdate.Add("CorrectRatio", correctRatio);
            propertiesToUpdate.Add("PenalRatio", penalRatio);

            await _attemptRepository.SetProperties(attempt.Id, propertiesToUpdate);

            return (OperationErrorMessages.NoError, null);
        }
        #endregion
    }
}