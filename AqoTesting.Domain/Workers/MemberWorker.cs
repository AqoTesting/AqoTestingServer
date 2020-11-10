using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.DTOs.DB.Members;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Collections.Generic;

namespace AqoTesting.Domain.Workers
{
    public static class MemberWorker
    {
        #region GetMemberFromRoom
        /// <summary>
        /// Получение пользователя из комнаты по id комнаты и логину
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="memberLogin"></param>
        /// <returns>пользователь или null</returns>
        public static Member? GetMemberFromRoom(ObjectId roomId, string memberLogin)
        {
            var loginFilter = Builders<Member>.Filter.Eq("Login", memberLogin);
            var roomidfilter = Builders<Member>.Filter.Eq("RoomId", roomId);
            var filter = loginFilter & roomidfilter;
            var member = MongoController.MemberCollection.Find(filter).SingleOrDefault();
            return member;
        }

        /// <summary>
        /// Получение пользователя из комнаты по логину
        /// </summary>
        /// <param name="room"></param>
        /// <param name="memberLogin"></param>
        /// <returns>пользователь или null</returns>
        public static Member? GetMemberFromRoom(Room room, string memberLogin)
        {
            return GetMemberFromRoom(room.Id, memberLogin);
        }

        /// <summary>
        /// Получение пользователя по id
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns>пользователь или null</returns>
        public static Member? GetMemberById(ObjectId memberId)
        {
            var filter = Builders<Member>.Filter.Eq("Id", memberId);
            var member = MongoController.MemberCollection.Find(filter).SingleOrDefault();
            return member;
        }

        public static Member[] GetMembersByIds(ObjectId[] memberIds)
        {
            var filter = Builders<Member>.Filter.In("Id", memberIds);
            var members = MongoController.MemberCollection.Find(filter).ToEnumerable();
            return members.ToArray();
        }
        #endregion

        #region CheckMemberInRoom
        /// <summary>
        /// Проверка находится ли пользователь в комнате
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="memberLogin"></param>
        /// <returns>true или false если пользователя не найдено</returns>
        public static bool CheckMemberInRoom(ObjectId roomId, string memberLogin)
        {
            return GetMemberFromRoom(roomId, memberLogin) != null;
        }
        #endregion

        #region IO
        /// <summary>
        /// Вставка пользователя в базу
        /// </summary>
        /// <param name="member"></param>
        /// <returns>id пользователя в базе</returns>
        public static ObjectId InsertMember(Member member)
        {
            MongoController.MemberCollection.InsertOne(member);
            return member.Id;
        }
        /// <summary>
        /// Вставка пользователя в базу
        /// </summary>
        /// <param name="member"></param>
        /// <returns>id пользователя в базе</returns>
        public static ObjectId Insert(this Member member) => InsertMember(member);
        /// <summary>
        /// Вставка списка пользователей в базу
        /// </summary>
        /// <param name="members"></param>
        /// <returns>id пользователей в базе</returns>
        public static ObjectId[] InsertMembers(Member[] members)
        {
            MongoController.MemberCollection.InsertMany(members);
            return members.Select(members => members.Id).ToArray();
        }
        /// <summary>
        /// Вставка списка пользователей в базу
        /// </summary>
        /// <param name="members"></param>
        /// <returns>id пользователей в базе</returns>
        public static ObjectId[] Insert(this Member[] members) => InsertMembers(members);
        /// <summary>
        /// Удаление пользователя из базы
        /// </summary>
        /// <param name="member"></param>
        /// <returns>Успех</returns>
        public static bool DeleteMember(Member member)
        {
            RoomWorker.RemoveMemberFromRoomById(member.RoomId, member.Id);
            var filter = Builders<Member>.Filter.Eq("Id", member.Id);
            var isDeleteSuccessful = MongoController.MemberCollection.DeleteOne(filter).DeletedCount == 1;

            return isDeleteSuccessful;
        }
        /// <summary>
        /// Удаление пользователя из базы
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns>Успех</returns>
        public static bool DeleteMember(ObjectId memberId)
        {
            var member = GetMemberById(memberId);
            if (member != null)
                return DeleteMember(member);
            return false;
        }
        /// <summary>
        /// Удаление пользователя из базы
        /// </summary>
        /// <param name="member"></param>
        /// <returns>Успех</returns>
        public static bool DeleteFromDB(this Member member) => DeleteMember(member);

        #endregion

        #region Common
        /// <summary>
        /// Получение попытки прохождения теста
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="testId"></param>
        /// <returns>попытка или null</returns>
        public static Attempt? GetMemberAttempt(ObjectId memberId, ObjectId testId)
        {
            var member = GetMemberById(memberId);
            if (member != null)
                foreach (var attempt in member.Attempts)
                    if (attempt.TestId.Equals(testId))
                        return attempt;

            return null;
        }
        /// <summary>
        /// Получение попытки прохождения теста
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="memberLogin"></param>
        /// <param name="testId"></param>
        /// <returns>попытка или null</returns>
        public static Attempt? GetMemberAttempt(ObjectId roomId, string memberLogin, ObjectId testId)
        {
            var member = GetMemberFromRoom(roomId, memberLogin);
            if (member != null)
                foreach (var attempt in member.Attempts)
                    if (attempt.TestId.Equals(testId))
                        return attempt;

            return null;
        }
        /// <summary>
        /// Получение полей пользователя
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns>Поля или null, если пользователь не найден</returns>
        public static Dictionary<string, string>? GetMemberFields(ObjectId memberId)
        {
            var member = GetMemberById(memberId);

            return member?.Fields;
        }
        /// <summary>
        /// Получение полей пользователя
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="memberLogin"></param>
        /// <returns>Поля или null, если пользователь не найден</returns>
        public static Dictionary<string, string>? GetMemberFields(ObjectId roomId, string memberLogin)
        {
            var member = GetMemberFromRoom(roomId, memberLogin);

            return member?.Fields;
        }
        /// <summary>
        /// Установка полей пользователя
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="fields"></param>
        public static void SetMemberFields(ObjectId memberId, BsonDocument fields)
        {
            var filter = Builders<Member>.Filter.Eq("Id", memberId);
            var update = Builders<Member>.Update.Set("Fields", fields);
            MongoController.MemberCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Получение хеша пароля пользователя
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="memberLogin"></param>
        /// <returns>Хеш или null, если пользователь не найден</returns>
        public static byte[]? GetMemberPasswordHash(ObjectId roomId, string memberLogin)
        {
            var member = GetMemberFromRoom(roomId, memberLogin);

            return member?.PasswordHash;
        }
        /// <summary>
        /// Получение хеша пароля пользователя
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns>Хеш или null, если пользователь не найден</returns>
        public static byte[]? GetMemberPasswordHash(ObjectId memberId)
        {
            var member = GetMemberById(memberId);

            return member?.PasswordHash;
        }
        /// <summary>
        /// Установка хеша пароля пользователя
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="newPasswordHash"></param>
        public static void SetMemberPasswordHash(ObjectId memberId, byte[] newPasswordHash)
        {
            var filter = Builders<Member>.Filter.Eq("Id", memberId);
            var update = Builders<Member>.Update.Set("PasswordHash", newPasswordHash);
            MongoController.MemberCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Установка хеша пароля пользователя
        /// </summary>
        /// <param name="member"></param>
        /// <param name="newPasswordHash"></param>
        public static void SetPasswordHash(this Member member, byte[] newPasswordHash)
        {
            member.PasswordHash = newPasswordHash;
            SetMemberPasswordHash(member.Id, newPasswordHash);
        }
        /// <summary>
        /// Установка id комнаты к которой относится пользователь (только для использования внутри "ДОМЕНА" или отладки!!!)
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="newRoomId"></param>
        /// <returns></returns>
        public static bool SetMemberRoomId(ObjectId memberId, ObjectId newRoomId)
        {
            var filter = Builders<Member>.Filter.Eq("Id", memberId);
            var update = Builders<Member>.Update.Set("RoomId", newRoomId);
            var isModifySuccessful = MongoController.MemberCollection.UpdateOne(filter, update).ModifiedCount == 1;

            return isModifySuccessful;
        }
        /// <summary>
        /// Установка id комнаты к которой относится пользователь (только для использования внутри "ДОМЕНА" или отладки!!!)
        /// </summary>
        /// <param name="member"></param>
        /// <param name="newRoomId"></param>
        /// <returns></returns>
        public static bool SetRoomId(this Member member, ObjectId newRoomId)
        {
            var roomIdChanged = SetMemberRoomId(member.Id, newRoomId);
            if (roomIdChanged == true)
                member.RoomId = newRoomId;
            return roomIdChanged;
        }
        #endregion
    }
}
