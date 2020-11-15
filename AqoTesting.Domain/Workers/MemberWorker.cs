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
        /// Получения участников по id комнаты
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public static Member[]? GetMembersFromRoom(ObjectId roomId)
        {
            var filter = Builders<Member>.Filter.Eq("RoomId", roomId);
            var members = MongoController.MemberCollection.Find(filter).ToList().ToArray();
            return members;
        }

        /// <summary>
        /// Получения участников из комнаты
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static Member[]? GetMembersFromRoom(Room room)
        {
            return GetMembersFromRoom(room.Id);
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

        /// <summary>
        /// Получение мембера по данным авторизации
        /// </summary>
        /// <param name="login"></param>
        /// <param name="passwordHash"></param>
        /// <returns>Мембер или null</returns>
        public static Member? GetMemberByAuthData(string login, byte[] passwordHash)
        {
            var loginFilter = Builders<Member>.Filter.Eq("Email", login) | Builders<Member>.Filter.Eq("Login", login);
            var passwordFilter = Builders<Member>.Filter.Eq("PasswordHash", passwordHash);
            var filter = loginFilter & passwordFilter;
            var user = MongoController.MemberCollection.Find(filter).SingleOrDefault();

            return user;
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
        /// Получение участника по id комнаты и полям
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="fields"></param>
        /// <returns>Объект участника</returns>
        public static Member? GetMemberByFields(ObjectId roomId, Dictionary<string, string> fields)
        {
            var roomIdFilter = Builders<Member>.Filter.Eq("RoomId", roomId);
            var fieldsFilter = Builders<Member>.Filter.Eq("Fields", fields);
            var filter = roomIdFilter & fieldsFilter;
            var member = MongoController.MemberCollection.Find(filter).SingleOrDefault();

            return member;
        }

        /// <summary>
        /// Получение участника по id комнаты и хешу полей
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="fieldsHash"></param>
        /// <returns></returns>
        public static Member? GetMemberByFields(ObjectId roomId, byte[] fieldsHash)
        {
            var roomIdFilter = Builders<Member>.Filter.Eq("RoomId", roomId);
            var fieldsFilter = Builders<Member>.Filter.Eq("FieldsHash", fieldsHash);
            var filter = roomIdFilter & fieldsFilter;
            var member = MongoController.MemberCollection.Find(filter).SingleOrDefault();

            return member;
        }
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

        /// <summary>
        /// Устанавливает IsChecked параметр пользователя
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="newIsChecked"></param>
        /// <returns></returns>
        public static bool SetMemberIsChecked(ObjectId memberId, bool newIsChecked)
        {
            var filter = Builders<Member>.Filter.Eq("Id", memberId);
            var update = Builders<Member>.Update.Set("IsChecked", newIsChecked);
            var isModifySuccessful = MongoController.MemberCollection.UpdateOne(filter, update).ModifiedCount == 1;

            return isModifySuccessful;
        }
        /// <summary>
        /// Устанавливает IsChecked параметр пользователя
        /// </summary>
        /// <param name="member"></param>
        /// <param name="newIsChecked"></param>
        /// <returns></returns>
        public static bool SetIsChecked(this Member member, bool newIsChecked)
        {
            var dbUpdateSuccessful = SetMemberIsChecked(member.Id, newIsChecked);
            if (dbUpdateSuccessful == true)
                member.IsChecked = newIsChecked;
            return dbUpdateSuccessful;
        }

        /// <summary>
        /// Устанавливает IsRegistred параметр пользователя
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="newIsRegistred"></param>
        /// <returns></returns>
        public static bool SetMemberIsRegistered(ObjectId memberId, bool newIsRegistred)
        {
            var filter = Builders<Member>.Filter.Eq("Id", memberId);
            var update = Builders<Member>.Update.Set("IsRegistered", newIsRegistred);
            var isModifySuccessful = MongoController.MemberCollection.UpdateOne(filter, update).ModifiedCount == 1;

            return isModifySuccessful;
        }
        /// <summary>
        /// Устанавливает IsRegistred параметр пользователя
        /// </summary>
        /// <param name="member"></param>
        /// <param name="newIsRegistred"></param>
        /// <returns></returns>
        public static bool SetIsRegistred(this Member member, bool newIsRegistred)
        {
            var dbUpdateSuccessful = SetMemberIsRegistered(member.Id, newIsRegistred);
            if (dbUpdateSuccessful == true)
                member.IsRegistered = newIsRegistred;
            return dbUpdateSuccessful;
        }
        /// <summary>
        /// Устанавливает FieldsHash параметр пользователя
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="newFieldsHash"></param>
        /// <returns></returns>
        public static bool SetMemberFieldsHash(ObjectId memberId, byte[] newFieldsHash)
        {
            var filter = Builders<Member>.Filter.Eq("Id", memberId);
            var update = Builders<Member>.Update.Set("FieldsHash", newFieldsHash);
            var isModifySuccessful = MongoController.MemberCollection.UpdateOne(filter, update).ModifiedCount == 1;

            return isModifySuccessful;
        }
        /// <summary>
        /// Устанавливает FieldsHash параметр пользователя
        /// </summary>
        /// <param name="member"></param>
        /// <param name="newFieldsHash"></param>
        /// <returns></returns>
        public static bool SetFieldsHash(this Member member, byte[] newFieldsHash)
        {
            var dbUpdateSuccessful = SetMemberFieldsHash(member.Id, newFieldsHash);
            if (dbUpdateSuccessful == true)
                member.FieldsHash = newFieldsHash;
            return dbUpdateSuccessful;
        }
        #endregion
    }
}
