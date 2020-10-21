﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.BD.Rooms;
using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces {
    public interface IRoomRespository {
        Task<Room[]> GetRoomsByOwnerId(ObjectId ownerId);
    }
}
