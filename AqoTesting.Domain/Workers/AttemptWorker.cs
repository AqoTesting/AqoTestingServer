using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AqoTesting.Domain.Workers
{
    public static class AttemptWorker
    {
        #region IO
        /// <summary>
        /// Вставляет попытку в базу
        /// </summary>
        /// <param name="attempt"></param>
        /// <returns></returns>
        public static ObjectId InsertAttempt(Attempt attempt)
        {
            MongoController.AttemptCollection?.InsertOne(attempt);

            return attempt.Id;
        }
        /// <summary>
        /// Вставляет попытку в базу
        /// </summary>
        /// <param name="attempt"></param>
        /// <returns></returns>
        public static ObjectId Insert(this Attempt attempt) => InsertAttempt(attempt);
        /// <summary>
        /// Добавляет попытки в базу
        /// </summary>
        /// <param name="attempts"></param>
        /// <returns></returns>
        public static ObjectId[] InsertAttempts(Attempt[] attempts)
        {
            MongoController.AttemptCollection?.InsertMany(attempts);
            return attempts.Select(attempts => attempts.Id).ToArray();
        }
        /// <summary>
        /// Добавляет попытки в базу
        /// </summary>
        /// <param name="attempts"></param>
        /// <returns></returns>
        public static ObjectId[] Insert(this Attempt[] attempts) => InsertAttempts(attempts);
        /// <summary>
        /// Замена попытки в базе
        /// </summary>
        /// <param name="attempt"></param>
        public static void ReplaceAttempt(Attempt attempt)
        {
            var filter = Builders<Attempt>.Filter.Eq("Id", attempt.Id);
            MongoController.AttemptCollection?.ReplaceOne(filter, attempt);
        }
        public static bool DeleteAttempt(ObjectId attemptId)
        {
            var filter = Builders<Attempt>.Filter.Eq("Id", attemptId);
            return MongoController.AttemptCollection?.DeleteOne(filter).DeletedCount == 1;
        }
        public static bool Delete(Attempt attempt) => DeleteAttempt(attempt.Id);
        #endregion
    }
}
