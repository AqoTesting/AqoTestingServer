﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.DB.Attempts;
using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces
{
    public interface IAttemptRepository
    {
        Task<AttemptsDB_AttemptDTO> GetAttemptById(ObjectId attemptId);

        Task<AttemptsDB_AttemptDTO> GetActiveAttemptByMemberId(ObjectId memberId);

        Task<AttemptsDB_AttemptDTO[]> GetAttemptsByTestId(ObjectId testId);
        Task<AttemptsDB_AttemptDTO[]> GetAttemptsByMemberId(ObjectId memberId);
        Task<AttemptsDB_AttemptDTO[]> GetAttemptsByTestIdAndMemberId(ObjectId testId, ObjectId memberId);

        Task<ObjectId> InsertAttempt(AttemptsDB_AttemptDTO newAttempt);

        Task<bool> SetProperty(ObjectId attemptId, string propertyName, object newPropertyValue);
        Task<bool> SetProperties(ObjectId attemptId, Dictionary<string, object> properties);

        Task<bool> Delete(ObjectId attemptId);
        Task<long> DeleteAttemptsByMemberId(ObjectId memberId);
        Task<long> DeleteAttemptsByRoomId(ObjectId roomId);
    }
}
