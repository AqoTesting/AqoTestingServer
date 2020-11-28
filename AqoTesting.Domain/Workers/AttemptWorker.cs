using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.DB.Attempts;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
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
            var isCompletedFilter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("IsActive", true);
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

        #region Properties
        public static async Task<bool> SetProperty(ObjectId attemptId, string propertyName, object newPropertyValue)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("Id", attemptId);
            var update = Builders<AttemptsDB_Attempt_DTO>.Update.Set(propertyName, newPropertyValue);

            return (await MongoController.AttemptCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetProperties(ObjectId attemptId, Dictionary<string, object> properties)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("Id", attemptId);
            var updates = new List<UpdateDefinition<AttemptsDB_Attempt_DTO>>();
            var update = Builders<AttemptsDB_Attempt_DTO>.Update;
            foreach (KeyValuePair<string, object> property in properties)
                updates.Add(update.Set(property.Key, property.Value));

            return (await MongoController.AttemptCollection.UpdateOneAsync(filter, update.Combine(updates.ToArray()))).MatchedCount == 1;
        }

        #endregion
    }
}
