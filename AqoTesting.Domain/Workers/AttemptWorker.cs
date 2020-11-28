using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.DB.Attempts;
using AqoTesting.Shared.DTOs.DB.Tests;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
namespace AqoTesting.Domain.Workers
{
    public static class AttemptWorker
    {
        #region IO
        public static async Task<AttemptsDB_Attempt_DTO> GetAttemptById(ObjectId attemptId)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("Id", attemptId);
            var attempt = await MongoController.AttemptCollection.Find(filter).SingleOrDefaultAsync();

            return attempt;
        }

        public static async Task<AttemptsDB_Attempt_DTO> GetActiveAttemptByMemberId(ObjectId memberId)
        {
            var isCompletedFilter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("IsCompleted", false);
            var memberIdFilter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("MemberId", memberId);
            var filter = isCompletedFilter & memberIdFilter;
            var attempt = await MongoController.AttemptCollection.Find(filter).SingleOrDefaultAsync();

            return attempt;
        }

        public static async Task<AttemptsDB_Attempt_DTO[]> GetAttemptsByTestId(ObjectId attemptId)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("TestId", attemptId);
            var attempts = await MongoController.AttemptCollection.Find(filter).ToListAsync();

            return attempts.ToArray();
        }

        public static async Task<AttemptsDB_Attempt_DTO[]> GetAttemptsByMemberId(ObjectId memberId)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("MemberId", memberId);
            var attempts = await MongoController.AttemptCollection.Find(filter).ToListAsync();

            return attempts.ToArray();
        }

        public static async Task<AttemptsDB_Attempt_DTO[]> GetAttemptsByTestIdAndMemberId(ObjectId testId, ObjectId memberId)
        {
            var testIdFilter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("TestId", testId);
            var memberIdFilter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("MemberId", memberId);
            var filter = testIdFilter & memberIdFilter;
            var attempts = await MongoController.AttemptCollection.Find(filter).ToListAsync();

            return attempts.ToArray();
        }

        public static async Task<ObjectId> InsertAttempt(AttemptsDB_Attempt_DTO attempt)
        {
            await MongoController.AttemptCollection.InsertOneAsync(attempt);

            return attempt.Id;
        }
        public static async Task<ObjectId> Insert(this AttemptsDB_Attempt_DTO attempt) =>
            await InsertAttempt(attempt);
        
        public static async Task<ObjectId[]> InsertAttempts(AttemptsDB_Attempt_DTO[] attempts)
        {
            await MongoController.AttemptCollection.InsertManyAsync(attempts);
            return attempts.Select(attempts => attempts.Id).ToArray();
        }
        public static async Task<ObjectId[]> Insert(this AttemptsDB_Attempt_DTO[] attempts) =>
            await InsertAttempts(attempts);
        
        public static async Task ReplaceAttempt(AttemptsDB_Attempt_DTO attempt)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("Id", attempt.Id);
            await MongoController.AttemptCollection.ReplaceOneAsync(filter, attempt);
        }
        
        public static async Task<bool> DeleteAttempt(ObjectId attemptId)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("Id", attemptId);

            return (await MongoController.AttemptCollection.DeleteOneAsync(filter)).DeletedCount == 1;
        }
        public static async Task<bool> Delete(AttemptsDB_Attempt_DTO attempt) =>
            await DeleteAttempt(attempt.Id);
        
        public static async Task<long?> DeleteAttemptsByMemberId(ObjectId memberId)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("MemberId", memberId);
            return (await MongoController.AttemptCollection.DeleteManyAsync(filter)).DeletedCount;
        }
        #endregion

        #region Props
        public static async Task<bool> SetAttemptTestId(ObjectId attemptId, ObjectId newTestId)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("Id", attemptId);
            var update = Builders<AttemptsDB_Attempt_DTO>.Update.Set("TestId", newTestId);
            return (await MongoController.AttemptCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetTestId(AttemptsDB_Attempt_DTO attempt, ObjectId newTestId)
        {
            var updated = await SetAttemptTestId(attempt.Id, newTestId);
            if (updated == true)
                attempt.TestId = newTestId;
            return updated;
        }

        public static async Task<bool> SetAttemptSeed(ObjectId attemptId, int newSeed)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("Id", attemptId);
            var update = Builders<AttemptsDB_Attempt_DTO>.Update.Set("Seed", newSeed);
            return (await MongoController.AttemptCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetAttemptSections(ObjectId attemptId, TestsDB_Section_DTO[] newSections)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("Id", attemptId);
            var update = Builders<AttemptsDB_Attempt_DTO>.Update.Set("Sections", newSections);
            return (await MongoController.AttemptCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetAttemptStartDate(ObjectId attemptId, DateTime newStartDate)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("Id", attemptId);
            var update = Builders<AttemptsDB_Attempt_DTO>.Update.Set("StartDate", newStartDate);
            return (await MongoController.AttemptCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetStartDate(AttemptsDB_Attempt_DTO attempt, DateTime newStartDate)
        {
            var updated = await SetAttemptStartDate(attempt.Id, newStartDate);
            if (updated == true)
                attempt.StartDate = newStartDate;
            return updated;
        }

        public static async Task<bool> SetAttemptEndDate(ObjectId attemptId, DateTime newEndDate)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("Id", attemptId);
            var update = Builders<AttemptsDB_Attempt_DTO>.Update.Set("EndDate", newEndDate);
            return (await MongoController.AttemptCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetEndDate(AttemptsDB_Attempt_DTO attempt, DateTime newEndDate)
        {
            var updated = await SetAttemptEndDate(attempt.Id, newEndDate);
            if (updated == true)
                attempt.EndDate = newEndDate;
            return updated;
        }

        public static async Task<bool> SetAttemptIsCompleted(ObjectId attemptId, bool newIsCompleted)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("Id", attemptId);
            var update = Builders<AttemptsDB_Attempt_DTO>.Update.Set("IsCompleted", newIsCompleted);
            return (await MongoController.AttemptCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetSections(AttemptsDB_Attempt_DTO attempt, bool newIsCompleted)
        {
            var updated = await SetAttemptIsCompleted(attempt.Id, newIsCompleted);
            if (updated == true)
                attempt.IsCompleted = newIsCompleted;
            return updated;
        }

        public static async Task<bool> SetAttemptScore(ObjectId attemptId, int newScore)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("Id", attemptId);
            var update = Builders<AttemptsDB_Attempt_DTO>.Update.Set("Cost", newScore);
            return (await MongoController.AttemptCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetSections(AttemptsDB_Attempt_DTO attempt, int newScore)
        {
            var updated = await SetAttemptScore(attempt.Id, newScore);
            if (updated == true)
                attempt.Score = newScore;
            return updated;
        }

        #endregion
    }
}
