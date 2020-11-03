using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.DTOs.DB.Members;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;

namespace AqoTesting.Domain.Workers
{
    public static class MemberWorker
    {
        #region GetMemberFromRoom

        public static Member? GetMemberFromRoom(ObjectId roomId, string memberLogin)
        {
            var loginFilter = Builders<Member>.Filter.Eq("Login", memberLogin);
            var roomidfilter = Builders<Member>.Filter.Eq("RoomId", roomId);
            var filter = loginFilter & roomidfilter;
            var member = MongoController.MemberCollection.Find(filter).SingleOrDefault();
            return member;
        }

        public static Member? GetMemberFromRoom(Room room, string memberLogin)
        {
            return GetMemberFromRoom(room.Id, memberLogin);
        }

        public static Member? GetMemberById(ObjectId memberId)
        {
            var filter = Builders<Member>.Filter.Eq("Id", memberId);
            var member = MongoController.MemberCollection.Find(filter).SingleOrDefault();
            return member;
        }
        #endregion

        #region CheckMemberInRoom

        public static bool CheckMemberInRoom(ObjectId roomId, string memberLogin)
        {
            return GetMemberFromRoom(roomId, memberLogin) != null;
        }
        #endregion

        #region IO

        public static ObjectId InsertMember(Member member)
        {
            MongoController.MemberCollection.InsertOne(member);
            return member.Id;
        }
        public static ObjectId Insert(this Member member) => InsertMember(member);

        public static ObjectId[] InsertMembers(Member[] members)
        {
            MongoController.MemberCollection.InsertMany(members);
            return members.Select(members => members.Id).ToArray();
        }
        public static ObjectId[] Insert(this Member[] members) => InsertMembers(members);

        public static bool DeleteMember(Member member)
        {
            RoomWorker.RemoveMemberFromRoomById(member.RoomId, member.Id);
            var filter = Builders<Member>.Filter.Eq("Id", member.Id);
            var isDeleteSuccessful = MongoController.MemberCollection.DeleteOne(filter).DeletedCount == 1;

            return isDeleteSuccessful;
        }
        public static bool DeleteMember(ObjectId memberId)
        {
            var member = GetMemberById(memberId);
            if (member != null)
                return DeleteMember(member);
            return false;
        }
        public static bool Delete(this Member member) => DeleteMember(member);

        #endregion

        #region Common
        public static Attempt? GetMemberAttempt(ObjectId roomId, string memberLogin, ObjectId testId)
        {
            var member = GetMemberFromRoom(roomId, memberLogin);
            if (member != null)
                foreach (var attempt in member.Attempts)
                    if (attempt.TestId.Equals(testId))
                        return attempt;

            return null;
        }

        public static object? GetMemberUserData(ObjectId roomId, string memberLogin)
        {
            var member = GetMemberFromRoom(roomId, memberLogin);

            return member?.UserData;
        }

        public static object? GetMemberPassword(ObjectId roomId, string memberLogin)
        {
            var member = GetMemberFromRoom(roomId, memberLogin);

            return member?.PasswordHash;
        }

        public static bool SetMemberRoomId(ObjectId memberId, ObjectId newRoomId)
        {
            var filter = Builders<Member>.Filter.Eq("Id", memberId);
            var update = Builders<Member>.Update.Set("RoomId", newRoomId);
            var isModifySuccessful = MongoController.MemberCollection.UpdateOne(filter, update).ModifiedCount == 1;

            return isModifySuccessful;
        }
        #endregion
    }
}
