using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;

namespace AqoTesting.Shared.Infrastructure
{
    public class WorkContext : IWorkContext
    {
        public ObjectId UserId { get; set; }
        public ObjectId MemberId { get; set; }
        public ObjectId RoomId { get; set; } = ObjectId.Empty;
    }
}
