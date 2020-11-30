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
        public static async Task<AttemptsDB_AttemptDTO> GetAttemptById(ObjectId attemptId)
        {
            var filter = Builders<AttemptsDB_AttemptDTO>.Filter.Eq("Id", attemptId);
            var attempt = await MongoController.AttemptCollection.Find(filter).SingleOrDefaultAsync();

            return attempt;
        }

        public static async Task<AttemptsDB_AttemptDTO> GetActiveAttemptByMemberId(ObjectId memberId)
        {
            var isCompletedFilter = Builders<AttemptsDB_AttemptDTO>.Filter.Eq("IsActive", true);
            var memberIdFilter = Builders<AttemptsDB_AttemptDTO>.Filter.Eq("MemberId", memberId);
            var filter = isCompletedFilter & memberIdFilter;
            var attempt = await MongoController.AttemptCollection.Find(filter).SingleOrDefaultAsync();

            return attempt;
        }

        public static async Task<AttemptsDB_AttemptDTO[]> GetAttemptsByTestId(ObjectId attemptId)
        {
            var filter = Builders<AttemptsDB_AttemptDTO>.Filter.Eq("TestId", attemptId);
            var attempts = await MongoController.AttemptCollection.Find(filter).ToListAsync();

            return attempts.ToArray();
        }

        public static async Task<AttemptsDB_AttemptDTO[]> GetAttemptsByMemberId(ObjectId memberId)
        {
            var filter = Builders<AttemptsDB_AttemptDTO>.Filter.Eq("MemberId", memberId);
            var attempts = await MongoController.AttemptCollection.Find(filter).ToListAsync();

            return attempts.ToArray();
        }

        public static async Task<AttemptsDB_AttemptDTO[]> GetAttemptsByTestIdAndMemberId(ObjectId testId, ObjectId memberId)
        {
            var testIdFilter = Builders<AttemptsDB_AttemptDTO>.Filter.Eq("TestId", testId);
            var memberIdFilter = Builders<AttemptsDB_AttemptDTO>.Filter.Eq("MemberId", memberId);
            var filter = testIdFilter & memberIdFilter;
            var attempts = await MongoController.AttemptCollection.Find(filter).ToListAsync();

            return attempts.ToArray();
        }

        public static async Task<AttemptsDB_AttemptDTO[]> GetAttemptsByRoomId(ObjectId roomId)
        {
            var filter = Builders<AttemptsDB_AttemptDTO>.Filter.Eq("RoomId", roomId);
            var attempts = await MongoController.AttemptCollection.Find(filter).ToListAsync();

            return attempts.ToArray();
        }

        public static async Task<ObjectId> InsertAttempt(AttemptsDB_AttemptDTO attempt)
        {
            await MongoController.AttemptCollection.InsertOneAsync(attempt);

            return attempt.Id;
        }
        public static async Task<ObjectId> Insert(this AttemptsDB_AttemptDTO attempt) =>
            await InsertAttempt(attempt);
        
        public static async Task<ObjectId[]> InsertAttempts(AttemptsDB_AttemptDTO[] attempts)
        {
            await MongoController.AttemptCollection.InsertManyAsync(attempts);
            return attempts.Select(attempts => attempts.Id).ToArray();
        }
        public static async Task<ObjectId[]> Insert(this AttemptsDB_AttemptDTO[] attempts) =>
            await InsertAttempts(attempts);
        
        public static async Task ReplaceAttempt(AttemptsDB_AttemptDTO attempt)
        {
            var filter = Builders<AttemptsDB_AttemptDTO>.Filter.Eq("Id", attempt.Id);
            await MongoController.AttemptCollection.ReplaceOneAsync(filter, attempt);
        }
        
        public static async Task<bool> DeleteAttempt(ObjectId attemptId)
        {
            var filter = Builders<AttemptsDB_AttemptDTO>.Filter.Eq("Id", attemptId);

            return (await MongoController.AttemptCollection.DeleteOneAsync(filter)).DeletedCount == 1;
        }
        public static async Task<bool> Delete(AttemptsDB_AttemptDTO attempt) =>
            await DeleteAttempt(attempt.Id);
        
        public static async Task<long> DeleteAttemptsByMemberId(ObjectId memberId)
        {
            var filter = Builders<AttemptsDB_AttemptDTO>.Filter.Eq("MemberId", memberId);
            return (await MongoController.AttemptCollection.DeleteManyAsync(filter)).DeletedCount;
        }

        public static async Task<long> DeleteAttemptsByRoomId(ObjectId roomId)
        {
            var filter = Builders<AttemptsDB_AttemptDTO>.Filter.Eq("RoomId", roomId);
            return (await MongoController.AttemptCollection.DeleteManyAsync(filter)).DeletedCount;
        }
        #endregion

        #region Properties
        public static async Task<bool> SetProperty(ObjectId attemptId, string propertyName, object newPropertyValue)
        {
            var filter = Builders<AttemptsDB_AttemptDTO>.Filter.Eq("Id", attemptId);
            var update = Builders<AttemptsDB_AttemptDTO>.Update.Set(propertyName, newPropertyValue);

            return (await MongoController.AttemptCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetProperties(ObjectId attemptId, Dictionary<string, object> properties)
        {
            var filter = Builders<AttemptsDB_AttemptDTO>.Filter.Eq("Id", attemptId);
            var updates = new List<UpdateDefinition<AttemptsDB_AttemptDTO>>();
            var update = Builders<AttemptsDB_AttemptDTO>.Update;
            foreach (KeyValuePair<string, object> property in properties)
                updates.Add(update.Set(property.Key, property.Value));

            return (await MongoController.AttemptCollection.UpdateOneAsync(filter, update.Combine(updates.ToArray()))).MatchedCount == 1;
        }

        #endregion
    }
}
