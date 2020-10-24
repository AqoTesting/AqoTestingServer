using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces
{
    public interface IWorkContext
    {
        ObjectId UserId { get; set; }
    }
}
