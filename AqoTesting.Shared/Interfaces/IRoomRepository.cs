﻿using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.Interfaces.DTO;
using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces
{
    public interface IRoomRepository
    {
        Task<Room> GetRoomById(ObjectId roomId);

        Task<Room> GetRoomByDomain(string domain);

        Task<Room[]> GetRoomsByOwnerId(ObjectId ownerId);

        Task<ObjectId> InsertRoom(Room newRoom);

        Task SetRoomName(ObjectId roomId, string newName);
        Task SetRoomDomain(ObjectId roomId, string newDomain);
        Task SetRoomRequestedFields(ObjectId roomId, IUserRoomField[] newRequestedFields);
        Task SetRoomIsDataRequired(ObjectId roomId, bool newIsDataRequired);
        Task SetRoomIsActive(ObjectId roomId, bool newIsActive);

        Task<bool> RemoveMemberFromRoomById(ObjectId roomId, ObjectId memberId);

        Task<bool> DeleteRoomById(ObjectId roomId);
    }
}
