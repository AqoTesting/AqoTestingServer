using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.DTOs.API.UserAPI.Attempts;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AutoMapper;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.Core.Services
{
    public class AttemptService : ServiceBase, IAttemptService
    {
        IAttemptRepository _attemptRepository;
        IMemberRepository _memberRepository;
        ICacheRepository _cacheRepository;

        public AttemptService(IAttemptRepository attemptRepository, IMemberRepository memberRepository, ICacheRepository cacheRepository)
        {
            _attemptRepository = attemptRepository;
            _memberRepository = memberRepository;
            _cacheRepository = cacheRepository;
        }

        #region UserAPI
        public async Task<(OperationErrorMessages, object)> UserAPI_GetAttempt(ObjectId attemptId)
        {
            var attempt = await _attemptRepository.GetAttemptById(attemptId);
            if (attempt == null)
                return (OperationErrorMessages.AttemptNotFound, null);

            var getAttemptDTO = Mapper.Map<UserAPI_GetAttempt_DTO>(attempt);

            return (OperationErrorMessages.NoError, getAttemptDTO);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_GetAttempt(CommonAPI_AttemptId_DTO attemptIdDTO) =>
            await this.UserAPI_GetAttempt(ObjectId.Parse(attemptIdDTO.AttemptId));
        #endregion
    }
}