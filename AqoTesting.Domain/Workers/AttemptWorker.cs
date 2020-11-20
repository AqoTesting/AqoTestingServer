using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.DB.Attempts;
using AqoTesting.Shared.DTOs.DB.Tests;
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
        public static ObjectId InsertAttempt(AttemptsDB_Attempt_DTO attempt)
        {
            MongoController.AttemptCollection?.InsertOne(attempt);

            return attempt.Id;
        }
        /// <summary>
        /// Вставляет попытку в базу
        /// </summary>
        /// <param name="attempt"></param>
        /// <returns></returns>
        public static ObjectId Insert(this AttemptsDB_Attempt_DTO attempt) => InsertAttempt(attempt);
        /// <summary>
        /// Добавляет попытки в базу
        /// </summary>
        /// <param name="attempts"></param>
        /// <returns></returns>
        public static ObjectId[] InsertAttempts(AttemptsDB_Attempt_DTO[] attempts)
        {
            MongoController.AttemptCollection?.InsertMany(attempts);
            return attempts.Select(attempts => attempts.Id).ToArray();
        }
        /// <summary>
        /// Добавляет попытки в базу
        /// </summary>
        /// <param name="attempts"></param>
        /// <returns></returns>
        public static ObjectId[] Insert(this AttemptsDB_Attempt_DTO[] attempts) => InsertAttempts(attempts);
        /// <summary>
        /// Замена попытки в базе
        /// </summary>
        /// <param name="attempt"></param>
        public static void ReplaceAttempt(AttemptsDB_Attempt_DTO attempt)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("Id", attempt.Id);
            MongoController.AttemptCollection?.ReplaceOne(filter, attempt);
        }
        /// <summary>
        /// Удаление попытки из базы
        /// </summary>
        /// <param name="attemptId"></param>
        /// <returns></returns>
        public static bool DeleteAttempt(ObjectId attemptId)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("Id", attemptId);
            return MongoController.AttemptCollection?.DeleteOne(filter).DeletedCount == 1;
        }
        /// <summary>
        /// Удаление попытки из базы
        /// </summary>
        /// <param name="attempt"></param>
        /// <returns></returns>
        public static bool Delete(AttemptsDB_Attempt_DTO attempt) => DeleteAttempt(attempt.Id);
        /// <summary>
        /// Удаляет все попытки пользователя
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public static long? DeleteAllMemberAttempts(ObjectId memberId)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("MemberId", memberId);
            return MongoController.AttemptCollection?.DeleteMany(filter).DeletedCount;
        }
        #endregion

        #region Props
        /// <summary>
        /// Устанавливает Id теста
        /// </summary>
        /// <param name="attemptId"></param>
        /// <param name="newTestId"></param>
        /// <returns></returns>
        public static bool SetAttemptTestId(ObjectId attemptId, ObjectId newTestId)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("Id", attemptId);
            var update = Builders<AttemptsDB_Attempt_DTO>.Update.Set("TestId", newTestId);
            return MongoController.AttemptCollection?.UpdateOne(filter, update).MatchedCount == 1;
        }
        /// <summary>
        /// Устанавливает Id теста
        /// </summary>
        /// <param name="attempt"></param>
        /// <param name="newTestId"></param>
        /// <returns></returns>
        public static bool SetTestId(AttemptsDB_Attempt_DTO attempt, ObjectId newTestId)
        {
            var updated = SetAttemptTestId(attempt.Id, newTestId);
            if (updated == true)
                attempt.TestId = newTestId;
            return updated;
        }
        /// <summary>
        /// Устанавливает сид для попытки
        /// </summary>
        /// <param name="attemptId"></param>
        /// <param name="newSeed"></param>
        /// <returns></returns>
        public static bool SetAttemptSeed(ObjectId attemptId, int newSeed)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("Id", attemptId);
            var update = Builders<AttemptsDB_Attempt_DTO>.Update.Set("Seed", newSeed);
            return MongoController.AttemptCollection?.UpdateOne(filter, update).MatchedCount == 1;
        }
        /// <summary>
        /// Устанавливает секции в попытке
        /// </summary>
        /// <param name="attemptId"></param>
        /// <param name="newSections"></param>
        /// <returns></returns>
        public static bool SetAttemptSections(ObjectId attemptId, TestsDB_Section_DTO[] newSections)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("Id", attemptId);
            var update = Builders<AttemptsDB_Attempt_DTO>.Update.Set("Sections", newSections);
            return MongoController.AttemptCollection?.UpdateOne(filter, update).MatchedCount == 1;
        }
        /// <summary>
        /// Устанавливает дату и время начала в попытке
        /// </summary>
        /// <param name="attemptId"></param>
        /// <param name="newStartDate"></param>
        /// <returns></returns>
        public static bool SetAttemptStartDate(ObjectId attemptId, DateTime newStartDate)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("Id", attemptId);
            var update = Builders<AttemptsDB_Attempt_DTO>.Update.Set("StartDate", newStartDate);
            return MongoController.AttemptCollection?.UpdateOne(filter, update).MatchedCount == 1;
        }
        /// <summary>
        /// Устанавливает дату и время начала в попытке
        /// </summary>
        /// <param name="attempt"></param>
        /// <param name="newStartDate"></param>
        /// <returns></returns>
        public static bool SetStartDate(AttemptsDB_Attempt_DTO attempt, DateTime newStartDate)
        {
            var updated = SetAttemptStartDate(attempt.Id, newStartDate);
            if (updated == true)
                attempt.StartDate = newStartDate;
            return updated;
        }
        /// <summary>
        /// Устанавливает дату и время конца в попытке
        /// </summary>
        /// <param name="attemptId"></param>
        /// <param name="newEndDate"></param>
        /// <returns></returns>
        public static bool SetAttemptEndDate(ObjectId attemptId, DateTime newEndDate)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("Id", attemptId);
            var update = Builders<AttemptsDB_Attempt_DTO>.Update.Set("EndDate", newEndDate);
            return MongoController.AttemptCollection?.UpdateOne(filter, update).MatchedCount == 1;
        }
        /// <summary>
        /// Устанавливает дату и время конца в попытке
        /// </summary>
        /// <param name="attempt"></param>
        /// <param name="newEndDate"></param>
        /// <returns></returns>
        public static bool SetEndDate(AttemptsDB_Attempt_DTO attempt, DateTime newEndDate)
        {
            var updated = SetAttemptEndDate(attempt.Id, newEndDate);
            if (updated == true)
                attempt.EndDate = newEndDate;
            return updated;
        }
        /// <summary>
        /// Устанавливает, завершена ли попытка
        /// </summary>
        /// <param name="attemptId"></param>
        /// <param name="newIsCompleted"></param>
        /// <returns></returns>
        public static bool SetAttemptIsCompleted(ObjectId attemptId, bool newIsCompleted)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("Id", attemptId);
            var update = Builders<AttemptsDB_Attempt_DTO>.Update.Set("IsCompleted", newIsCompleted);
            return MongoController.AttemptCollection?.UpdateOne(filter, update).MatchedCount == 1;
        }
        /// <summary>
        /// Устанавливает, завершена ли попытка
        /// </summary>
        /// <param name="attempt"></param>
        /// <param name="newIsCompleted"></param>
        /// <returns></returns>
        public static bool SetSections(AttemptsDB_Attempt_DTO attempt, bool newIsCompleted)
        {
            var updated = SetAttemptIsCompleted(attempt.Id, newIsCompleted);
            if (updated == true)
                attempt.IsCompleted = newIsCompleted;
            return updated;
        }
        /// <summary>
        /// Устанавливает счет попытки
        /// </summary>
        /// <param name="attemptId"></param>
        /// <param name="newScore"></param>
        /// <returns></returns>
        public static bool SetAttemptScore(ObjectId attemptId, int newScore)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("Id", attemptId);
            var update = Builders<AttemptsDB_Attempt_DTO>.Update.Set("Cost", newScore);
            return MongoController.AttemptCollection?.UpdateOne(filter, update).MatchedCount == 1;
        }
        /// <summary>
        /// Устанавливает счет попытки
        /// </summary>
        /// <param name="attempt"></param>
        /// <param name="newScore"></param>
        /// <returns></returns>
        public static bool SetSections(AttemptsDB_Attempt_DTO attempt, int newScore)
        {
            var updated = SetAttemptScore(attempt.Id, newScore);
            if (updated == true)
                attempt.Score = newScore;
            return updated;
        }

        #endregion
    }
}
