using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.DTOs.DB.Members;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Collections.Generic;
using AqoTesting.Shared.DTOs.DB.Attempts;

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
        public static MembersDB_Member_DTO? GetMemberFromRoom(ObjectId roomId, string memberLogin)
        {
            var loginFilter = Builders<MembersDB_Member_DTO>.Filter.Eq("Login", memberLogin);
            var roomidfilter = Builders<MembersDB_Member_DTO>.Filter.Eq("RoomId", roomId);
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
        public static MembersDB_Member_DTO? GetMemberFromRoom(RoomsDB_Room_DTO room, string memberLogin)
        {
            return GetMemberFromRoom(room.Id, memberLogin);
        }

        /// <summary>
        /// Получения участников по id комнаты
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public static MembersDB_Member_DTO[]? GetMembersFromRoom(ObjectId roomId)
        {
            var filter = Builders<MembersDB_Member_DTO>.Filter.Eq("RoomId", roomId);
            var members = MongoController.MemberCollection.Find(filter).ToList().ToArray();
            return members;
        }

        /// <summary>
        /// Получения участников из комнаты
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static MembersDB_Member_DTO[]? GetMembersFromRoom(RoomsDB_Room_DTO room)
        {
            return GetMembersFromRoom(room.Id);
        }


        /// <summary>
        /// Получение пользователя по id
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns>пользователь или null</returns>
        public static MembersDB_Member_DTO? GetMemberById(ObjectId memberId)
        {
            var filter = Builders<MembersDB_Member_DTO>.Filter.Eq("Id", memberId);
            var member = MongoController.MemberCollection.Find(filter).SingleOrDefault();
            return member;
        }

        public static MembersDB_Member_DTO[] GetMembersByIds(ObjectId[] memberIds)
        {
            var filter = Builders<MembersDB_Member_DTO>.Filter.In("Id", memberIds);
            var members = MongoController.MemberCollection.Find(filter).ToEnumerable();
            return members.ToArray();
        }

        /// <summary>
        /// Проверка данных авторизации мембера
        /// </summary>
        /// <param name="login"></param>
        /// <param name="passwordHash"></param>
        /// <returns>bool</returns>
        public static MembersDB_Member_DTO GetMemberByAuthData(ObjectId roomId, string login, byte[] passwordHash)
        {
            var roomIdFilter = Builders<MembersDB_Member_DTO>.Filter.Eq("RoomId", roomId);
            var loginFilter = Builders<MembersDB_Member_DTO>.Filter.Eq("Email", login) | Builders<MembersDB_Member_DTO>.Filter.Eq("Login", login);
            var passwordFilter = Builders<MembersDB_Member_DTO>.Filter.Eq("PasswordHash", passwordHash);
            var filter = roomIdFilter & loginFilter & passwordFilter;

            var member = MongoController.MemberCollection.Find(filter).SingleOrDefault();

            return member;
        }

        /// <summary>
        /// Получение мембера по хешу полей
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="fieldsHash"></param>
        /// <returns>bool</returns>
        public static MembersDB_Member_DTO GetMemberByFieldsHash(ObjectId roomId, byte[] fieldsHash)
        {
            var roomIdFilter = Builders<MembersDB_Member_DTO>.Filter.Eq("RoomId", roomId);
            var fieldsHashFilter = Builders<MembersDB_Member_DTO>.Filter.Eq("FieldsHash", fieldsHash);
            var filter = roomIdFilter & fieldsHashFilter;
            var member = MongoController.MemberCollection.Find(filter).SingleOrDefault();

            return member;
        }
        #endregion

        #region CheckMemberInRoom
        /// <summary>
        /// Проверка существования мембера в комнате по логину
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="login"></param>
        /// <returns>bool</returns>
        public static bool CheckMemberInRoomByLogin(ObjectId roomId, string login)
        {
            var roomIdFilter = Builders<MembersDB_Member_DTO>.Filter.Eq("RoomId", roomId);
            var loginFilter = Builders<MembersDB_Member_DTO>.Filter.Eq("Login", login);
            var filter = roomIdFilter & loginFilter;
            var member = MongoController.MemberCollection.Find(filter).SingleOrDefault();

            return member != null;
        }

        /// <summary>
        /// Проверка существования мембера в комнате по имейлу
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="email"></param>
        /// <returns>bool</returns>
        public static bool CheckMemberInRoomByEmail(ObjectId roomId, string email)
        {
            var roomIdFilter = Builders<MembersDB_Member_DTO>.Filter.Eq("RoomId", roomId);
            var emailFilter = Builders<MembersDB_Member_DTO>.Filter.Eq("Email", email);
            var filter = roomIdFilter & emailFilter;
            var member = MongoController.MemberCollection.Find(filter).SingleOrDefault();

            return member != null;
        }
        #endregion

        #region IO
        /// <summary>
        /// Вставка пользователя в базу
        /// </summary>
        /// <param name="member"></param>
        /// <returns>id пользователя в базе</returns>
        public static ObjectId InsertMember(MembersDB_Member_DTO member)
        {
            MongoController.MemberCollection?.InsertOne(member);

            return member.Id;
        }
        /// <summary>
        /// Вставка пользователя в базу
        /// </summary>
        /// <param name="member"></param>
        /// <returns>id пользователя в базе</returns>
        public static ObjectId Insert(this MembersDB_Member_DTO member) => InsertMember(member);
        /// <summary>
        /// Вставка списка пользователей в базу
        /// </summary>
        /// <param name="members"></param>
        /// <returns>id пользователей в базе</returns>
        public static ObjectId[] InsertMembers(MembersDB_Member_DTO[] members)
        {
            MongoController.MemberCollection?.InsertMany(members);
            return members.Select(members => members.Id).ToArray();
        }
        /// <summary>
        /// Вставка списка пользователей в базу
        /// </summary>
        /// <param name="members"></param>
        /// <returns>id пользователей в базе</returns>
        public static ObjectId[] Insert(this MembersDB_Member_DTO[] members) => InsertMembers(members);

        /// <summary>
        /// Полная перезапись мембера (сам Бу-э-э-э)
        /// </summary>
        /// <param name="updatedMember"
        public static void ReplaceMember(MembersDB_Member_DTO updatedMember)
        {
            var filter = Builders<MembersDB_Member_DTO>.Filter.Eq("Id", updatedMember.Id);
            MongoController.MemberCollection?.ReplaceOne(filter, updatedMember);
        }

        /// <summary>
        /// Удаление пользователя из базы
        /// </summary>
        /// <param name="member"></param>
        /// <returns>Успех</returns>
        public static bool DeleteMember(MembersDB_Member_DTO member)
        {
            RoomWorker.RemoveMemberFromRoomById(member.RoomId, member.Id);
            var filter = Builders<MembersDB_Member_DTO>.Filter.Eq("Id", member.Id);
            var isDeleteSuccessful = MongoController.MemberCollection?.DeleteOne(filter).DeletedCount == 1;

            if (isDeleteSuccessful == true)
                AttemptWorker.DeleteAllMemberAttempts(member.Id);

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
            if(member != null)
                return DeleteMember(member);
            return false;
        }
        /// <summary>
        /// Удаление пользователя из базы
        /// </summary>
        /// <param name="member"></param>
        /// <returns>Успех</returns>
        public static bool DeleteFromDB(this MembersDB_Member_DTO member) => DeleteMember(member);

        #endregion

        #region Common
        /// <summary>
        /// Получение попыток прохождения теста
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="testId"></param>
        /// <returns></returns>
        public static AttemptsDB_Attempt_DTO[]? GetMemberAttempts(ObjectId memberId, ObjectId testId)
        {
            var memberFilter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("MemberId", memberId);
            var testFilter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("TestId", testId);
            var filter = memberFilter & testFilter;
            var attempts = MongoController.AttemptCollection.Find(filter).ToList().ToArray();
            return attempts;
        }

        public static AttemptsDB_Attempt_DTO[]? GetMemberAttempts(this MembersDB_Member_DTO member, ObjectId testId)
        {
            return GetMemberAttempts(member.Id, testId);
        }

        /// <summary>
        /// Получение всех попыток прохождения всех тестов пользователя
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public static AttemptsDB_Attempt_DTO[]? GetMemberAttempts(ObjectId memberId)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("MemberId", memberId);
            var attempts = MongoController.AttemptCollection.Find(filter).ToList().ToArray();
            return attempts;
        }

        /// <summary>
        /// Получение всех попыток прохождения всех тестов пользователя
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static AttemptsDB_Attempt_DTO[]? GetMemberAttempts(this MembersDB_Member_DTO member)
        {
            return GetMemberAttempts(member.Id);
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
        /// <param name="newValue"></param>
        public static void SetMemberPasswordHash(ObjectId memberId, byte[] newValue)
        {
            var filter = Builders<MembersDB_Member_DTO>.Filter.Eq("Id", memberId);
            var update = Builders<MembersDB_Member_DTO>.Update.Set("PasswordHash", newValue);
            MongoController.MemberCollection?.UpdateOne(filter, update);
        }
        /// <summary>
        /// Установка хеша пароля пользователя
        /// </summary>
        /// <param name="member"></param>
        /// <param name="newValue"></param>
        public static void SetPasswordHash(this MembersDB_Member_DTO member, byte[] newValue)
        {
            member.PasswordHash = newValue;
            SetMemberPasswordHash(member.Id, newValue);
        }
        /// <summary>
        /// Установка id комнаты к которой относится пользователь (только для использования внутри "ДОМЕНА" или отладки!!!)
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static bool SetMemberRoomId(ObjectId memberId, ObjectId newValue)
        {
            var filter = Builders<MembersDB_Member_DTO>.Filter.Eq("Id", memberId);
            var update = Builders<MembersDB_Member_DTO>.Update.Set("RoomId", newValue);
            var isModifySuccessful = MongoController.MemberCollection?.UpdateOne(filter, update).ModifiedCount == 1;

            return isModifySuccessful;
        }
        /// <summary>
        /// Установка id комнаты к которой относится пользователь (только для использования внутри "ДОМЕНА" или отладки!!!)
        /// </summary>
        /// <param name="member"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static bool SetRoomId(this MembersDB_Member_DTO member, ObjectId newValue)
        {
            var roomIdChanged = SetMemberRoomId(member.Id, newValue);
            if(roomIdChanged == true)
                member.RoomId = newValue;
            return roomIdChanged;
        }

        /// <summary>
        /// Устанавливает IsApproved параметр пользователя
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static bool SetIsApproved(ObjectId memberId, bool newValue)
        {
            var filter = Builders<MembersDB_Member_DTO>.Filter.Eq("Id", memberId);
            var update = Builders<MembersDB_Member_DTO>.Update.Set("IsApproved", newValue);
            var isModifySuccessful = MongoController.MemberCollection?.UpdateOne(filter, update).ModifiedCount == 1;

            return isModifySuccessful;
        }
        /// <summary>
        /// Устанавливает IsApproved параметр пользователя
        /// </summary>
        /// <param name="member"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static bool SetIsApproved(this MembersDB_Member_DTO member, bool newValue)
        {
            var dbUpdateSuccessful = SetIsApproved(member.Id, newValue);
            if(dbUpdateSuccessful == true)
                member.IsApproved = newValue;
            return dbUpdateSuccessful;
        }

        /// <summary>
        /// Устанавливает IsRegistred параметр пользователя
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static bool SetIsRegistered(ObjectId memberId, bool newValue)
        {
            var filter = Builders<MembersDB_Member_DTO>.Filter.Eq("Id", memberId);
            var update = Builders<MembersDB_Member_DTO>.Update.Set("IsRegistered", newValue);
            var isModifySuccessful = MongoController.MemberCollection?.UpdateOne(filter, update).ModifiedCount == 1;

            return isModifySuccessful;
        }
        /// <summary>
        /// Устанавливает IsRegistred параметр пользователя
        /// </summary>
        /// <param name="member"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static bool SetIsRegistered(this MembersDB_Member_DTO member, bool newValue)
        {
            var dbUpdateSuccessful = SetIsRegistered(member.Id, newValue);
            if(dbUpdateSuccessful == true)
                member.IsRegistered = newValue;
            return dbUpdateSuccessful;
        }

        /// <summary>
        /// Установка полей пользователя
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="newValue"></param>
        public static void SetMemberFields(ObjectId memberId, BsonDocument newValue)
        {
            var filter = Builders<MembersDB_Member_DTO>.Filter.Eq("Id", memberId);
            var update = Builders<MembersDB_Member_DTO>.Update.Set("Fields", newValue);
            MongoController.MemberCollection?.UpdateOne(filter, update);
        }

        /// <summary>
        /// Устанавливает FieldsHash параметр пользователя
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static bool SetMemberFieldsHash(ObjectId memberId, byte[] newValue)
        {
            var filter = Builders<MembersDB_Member_DTO>.Filter.Eq("Id", memberId);
            var update = Builders<MembersDB_Member_DTO>.Update.Set("FieldsHash", newValue);
            var isModifySuccessful = MongoController.MemberCollection?.UpdateOne(filter, update).ModifiedCount == 1;

            return isModifySuccessful;
        }

        /// <summary>
        /// Устанавливает FieldsHash параметр пользователя
        /// </summary>
        /// <param name="member"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static bool SetFieldsHash(this MembersDB_Member_DTO member, byte[] newValue)
        {
            var dbUpdateSuccessful = SetMemberFieldsHash(member.Id, newValue);
            if(dbUpdateSuccessful == true)
                member.FieldsHash = newValue;
            return dbUpdateSuccessful;
        }
        #endregion
    }
}
