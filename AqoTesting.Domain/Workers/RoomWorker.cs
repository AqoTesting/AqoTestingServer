using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.API.Users.Rooms;
using AqoTesting.Shared.DTOs.DB.Members;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.Interfaces.DTO;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AqoTesting.Domain.Workers
{
    public static class RoomWorker
    {
        #region IO
        /// <summary>
        /// Поиск комнаты по id, вернет комнату или null в случае если она не найдена.
        /// </summary>
        public static Room GetRoomById(ObjectId roomId)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var room = MongoController.RoomCollection.Find(filter).SingleOrDefault();
            return room;
        }
        /// <summary>
        /// Поиск комнаты по домену, вернет комнату или null в случае если она не найдена.
        /// </summary>
        public static Room GetRoomByDomain(string domain)
        {
            var filter = Builders<Room>.Filter.Eq("Domain", domain);
            var room = MongoController.RoomCollection.Find(filter).SingleOrDefault();
            return room;
        }
        /// <summary>
        /// Поиск комнат созданных юзером
        /// </summary>
        public static Room[] GetRoomsByOwnerId(ObjectId ownerId)
        {
            var filter = Builders<Room>.Filter.Eq("OwnerId", ownerId);
            var rooms = MongoController.RoomCollection.Find(filter).ToEnumerable();

            return rooms.ToArray();
        }
        /// <summary>
        /// Вставка комнаты в базу
        /// </summary>
        public static ObjectId InsertRoom(Room room)
        {
            MongoController.RoomCollection.InsertOne(room);
            return room.Id;
        }
        /// <summary>
        /// Вставка комнат в базу
        /// </summary>
        public static ObjectId[] InsertRooms(Room[] rooms)
        {
            MongoController.RoomCollection.InsertMany(rooms);
            return rooms.Select(room => room.Id).ToArray();
        }
        /// <summary>
        /// Полная перезапись комнаты (Бу-э-э-э)
        /// </summary>
        public static void ReplaceRoom(ObjectId roomId, Room updated)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            MongoController.RoomCollection.ReplaceOne(filter, updated);
        }
        /// <summary>
        /// Всё еще полная перезапись, но красивее
        /// </summary>
        public static void UpdateInDB(this Room room) => ReplaceRoom(room.Id, room);
        /// <summary>
        /// Удаление комнаты по Id
        /// </summary>
        public static bool DeleteRoomById(ObjectId roomId)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var isDeleteSuccessful = MongoController.RoomCollection.DeleteOne(filter).DeletedCount == 1;
            return isDeleteSuccessful;
        }
        #endregion

        #region Members
        /// <summary>
        /// Привязывает пользователя из базы к комнате
        /// </summary>
        public static void AddMemberToRoom(ObjectId roomId, ObjectId memberId)
        {
            MemberWorker.SetMemberRoomId(memberId, roomId);
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Push("Members", memberId);
            MongoController.RoomCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Привязывает пользователя из базы к комнате, добавляет пользователя в объект комнаты
        /// </summary>
        public static void AddMember(this Room room, ObjectId memberId)
        {
            var membersList = room.Members.ToList();
            membersList.Add(memberId);
            room.Members = membersList.ToArray();

            AddMemberToRoom(room.Id, memberId);
        }
        /// <summary>
        /// Добавляет нового пользователя в комнату (пользователя, которого нет в базе)
        /// </summary>
        public static void AddNewMemberToRoom(ObjectId roomId, Member member)
        {
            member.RoomId = roomId;
            MemberWorker.InsertMember(member);
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Push("Members", member.Id);
            MongoController.RoomCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Добавляет нового пользователя в комнату (пользователя, которого нет в базе), добавляет пользователя в объект комнаты
        /// </summary>
        public static void AddNewMember(this Room room, Member member)
        {
            var membersList = room.Members.ToList();
            membersList.Add(member.Id);
            room.Members = membersList.ToArray();
            AddNewMemberToRoom(room.Id, member);
        }
        /// <summary>
        /// Удаляет пользователя из комнаты
        /// </summary>
        public static bool RemoveMemberFromRoomById(ObjectId roomId, ObjectId memberId)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Pull("Members", memberId);
            var isRemovedSuccessful = MongoController.RoomCollection.UpdateOne(filter, update).ModifiedCount == 1;

            return isRemovedSuccessful;
        }
        /// <summary>
        /// Удаляет пользователя из комнаты, удаляет из объекта
        /// </summary>
        public static void RemoveMemberByIdById(this Room room, ObjectId memberId)
        {
            var tmpList = new List<ObjectId>();
            foreach (var _memberId in room.Members)
                if (_memberId != memberId)
                    tmpList.Add(_memberId);
            room.Members = tmpList.ToArray();
            RemoveMemberFromRoomById(room.Id, memberId);
        }
        /// <summary>
        /// Удаляет пользователя из комнаты (по логину)
        /// </summary>
        public static void RemoveMemberFromRoomByLogin(ObjectId roomId, string memberLogin)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var member = MemberWorker.GetMemberFromRoom(roomId, memberLogin);
            if (member != null)
            {
                var update = Builders<Room>.Update.Pull("Members", member.Id);
                MongoController.RoomCollection.UpdateOne(filter, update);
            }
        }
        /// <summary>
        /// Удаляет пользователя из комнаты (по логину), НЕ удаляет его из объекта комнаты
        /// </summary>
        public static void RemoveMemberByLogin(this Room room, string memberLogin) => RemoveMemberFromRoomByLogin(room.Id, memberLogin);

        #endregion

        #region Tests
        /// <summary>
        /// Добавляет тест в комнату
        /// </summary>
        /// <returns>успех</returns>
        public static bool AddTestToRoomById(ObjectId roomId, ObjectId testId)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Push("TestIds", testId);
            var isUpdatedSuccessful = MongoController.RoomCollection.UpdateOne(filter, update).ModifiedCount == 1;
            return isUpdatedSuccessful;
        }
        /// <summary>
        /// Добавляет тест в комнату
        /// </summary>
        /// <returns>успех</returns>
        public static bool AddTestById(this Room room, ObjectId testId)
        {
            var testAdded = AddTestToRoomById(room.Id, testId);
            if (testAdded == true)
            {
                var testsList = room.TestIds.ToList();
                testsList.Add(testId);
                room.TestIds = testsList.ToArray();
            }
            return testAdded;
        }
        /// <summary>
        /// Удаляет тест из комнаты
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="testId"></param>
        /// <returns>Успех</returns>
        public static bool RemoveTestFromRoomById(ObjectId roomId, ObjectId testId)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Pull("TestIds", testId);
            var isUpdatedSuccessful = MongoController.RoomCollection.UpdateOne(filter, update).ModifiedCount == 1;
            return isUpdatedSuccessful;
        }
        /// <summary>
        /// Удаляет тест из комнаты
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="testId"></param>
        /// <returns>Успех</returns>
        public static bool RemoveTestById(this Room room, ObjectId testId)
        {
            var testRemoved = RemoveTestFromRoomById(room.Id, testId);
            if (testRemoved == true)
            {
                var testsList = room.TestIds.ToList();
                foreach (var _testId in room.TestIds)
                {
                    if (_testId != testId)
                        testsList.Add(_testId);
                }
                room.TestIds = testsList.ToArray();
            }
            return testRemoved;
        }

        #endregion

        #region Props
        /// <summary>
        /// Устанавливает имя комнаты
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="newName"></param>
        public static void SetRoomName(ObjectId roomId, string newName)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Set("Name", newName);
            MongoController.RoomCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Устанавливает имя комнаты
        /// </summary>
        /// <param name="room"></param>
        /// <param name="newName"></param>
        public static void SetName(this Room room, string newName)
        {
            room.Name = newName;
            SetRoomName(room.Id, newName);
        }
        /// <summary>
        /// Устанавливает домен комнаты
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="newDomain"></param>
        public static void SetRoomDomain(ObjectId roomId, string newDomain)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Set("Domain", newDomain);
            MongoController.RoomCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Устанавливает домен комнаты
        /// </summary>
        /// <param name="room"></param>
        /// <param name="newDomain"></param>
        public static void SetDomain(this Room room, string newDomain)
        {
            room.Domain = newDomain;
            SetRoomDomain(room.Id, newDomain);
        }
        /// <summary>
        /// Устанавливает поля комнаты
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="newFields"></param>
        public static void SetRoomFields(ObjectId roomId, RoomField[] newFields)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Set("Fields", newFields);
            MongoController.RoomCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Устанавливает поля комнаты
        /// </summary>
        /// <param name="room"></param>
        /// <param name="newFields"></param>
        public static void SetRequestedFields(this Room room, RoomField[] newFields)
        {
            room.Fields = newFields;
            SetRoomFields(room.Id, newFields);
        }
        /// <summary>
        /// Устанавливает нужна ли дополнительная информация в комнате
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="newIsDataRequired"></param>
        public static void SetRoomIsDataRequired(ObjectId roomId, bool newIsDataRequired)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Set("IsDataRequired", newIsDataRequired);
            MongoController.RoomCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Устанавливает нужна ли дополнительная информация в комнате
        /// </summary>
        /// <param name="room"></param>
        /// <param name="newIsDataRequired"></param>
        public static void SetIsDataRequired(this Room room, bool newIsDataRequired)
        {
            room.IsDataRequired = newIsDataRequired;
            SetRoomIsDataRequired(room.Id, newIsDataRequired);
        }
        /// <summary>
        /// Устанавливает активна ли комната
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="newIsActive"></param>
        public static void SetRoomIsActive(ObjectId roomId, bool newIsActive)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Set("IsActive", newIsActive);
            MongoController.RoomCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Устанавливает активна ли комната
        /// </summary>
        /// <param name="room"></param>
        /// <param name="newIsActive"></param>
        public static void SetIsActive(this Room room, bool newIsActive)
        {
            room.IsActive = newIsActive;
            SetRoomIsActive(room.Id, newIsActive);
        }

        #endregion

        #region Fields
        /// <summary>
        /// Возвращает поля комнаты
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns>Поля комнаты</returns>
        public static RoomField[]? GetRoomFields(ObjectId roomId)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var room = MongoController.RoomCollection.Find(filter).Project<Room>("{ Fields:1}").SingleOrDefault();

            return room?.Fields;
        }
        /// <summary>
        /// Возвращает поля комнаты
        /// </summary>
        /// <param name="room"></param>
        /// <returns>Поля комнаты</returns>
        public static RoomField[]? GetFields(this Room room) => GetRoomFields(room.Id);
        /// <summary>
        /// Добавляет поле в комнату
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="field"></param>
        public static void AddFieldToRoom(ObjectId roomId, RoomField field)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Push("Fields", field);
            MongoController.RoomCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Добавляет поле в комнату
        /// </summary>
        /// <param name="room"></param>
        /// <param name="field"></param>
        public static void AddField(this Room room, RoomField field)
        {
            var fieldsList = room.Fields.ToList();
            fieldsList.Add(field);
            room.Fields = fieldsList.ToArray();
            AddFieldToRoom(room.Id, field);
        }
        /// <summary>
        /// Удаляет поле из комнаты
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="fieldName"></param>
        public static void RemoveFieldFromRoom(ObjectId roomId, string fieldName)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var fieldFilter = Builders<RoomField>.Filter.Eq("Name", fieldName);
            var update = Builders<Room>.Update.PullFilter("Fields", fieldFilter);
            MongoController.RoomCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Удаляет поле из комнаты
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="fieldName"></param>
        public static void RemoveField(this Room room, string fieldName)
        {
            var fieldsList = new List<RoomField>();
            foreach (var field in room.Fields)
                if (field.Name != fieldName)
                    fieldsList.Add(field);
            room.Fields = fieldsList.ToArray();
            RemoveFieldFromRoom(room.Id, fieldName);
        }
        #endregion
    }
}
