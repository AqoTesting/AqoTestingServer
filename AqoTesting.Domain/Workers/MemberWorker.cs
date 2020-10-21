﻿using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.BD.Rooms;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.Domain.Workers
{
    public static class MemberWorker
    {
        #region GetMemberFromRoom
        public static Member? GetMemberFromRoom(Room room, string memberLogin)
        {
            foreach (var member in room.Members)
                if (member.Login == memberLogin)
                    return member;

            return null;
        }

        public static Member? GetMemberFromRoom(ObjectId roomId, string memberLogin)
        {
            return GetMemberFromRoom(MongoIOController.GetRoomById(roomId), memberLogin);
        }
        #endregion

        #region CheckMemberInRoom

        public static bool CheckMemberInRoom(ObjectId roomId, string memberLogin)
        {
            return GetMemberFromRoom(MongoIOController.GetRoomById(roomId), memberLogin) != null;
        }
        #endregion

        #region AddMember
        public static bool AddMemberInRoom(ObjectId roomId, Member member)
        {
            return true;
        }
        #endregion

        #region Common
        public static Attempt? GetMemberAttempt(ObjectId roomId, string memberLogin, ObjectId testId)
        {
            var member = GetMemberFromRoom(MongoIOController.GetRoomById(roomId), memberLogin);
            if (member != null)
                foreach (var attempt in member.Attempts)
                    if (attempt.TestId.Equals(testId))
                        return attempt;
            return null;
        }

        public static object? GetMemberUserData(ObjectId roomId, string memberLogin)
        {
            var member = GetMemberFromRoom(MongoIOController.GetRoomById(roomId), memberLogin);
            return member?.UserData;
        }

        public static object? GetMemberPassword(ObjectId roomId, string memberLogin)
        {
            var member = GetMemberFromRoom(MongoIOController.GetRoomById(roomId), memberLogin);
            return member?.Password;
        }
        #endregion
    }
}