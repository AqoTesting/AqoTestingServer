﻿using AqoTesting.Shared.Enums;
using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces
{
    public interface IWorkContext
    {
        ObjectId UserId { get; set; }
        ObjectId MemberId { get; set; }
        ObjectId RoomId { get; set; }
        bool IsChecked { get; set; }
    }
}
