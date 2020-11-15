using MongoDB.Bson;

namespace AqoTesting.Domain.Utils
{
    public static class DBUtils
    {
        public static ObjectId? ParseObjectId(string stringId)
        {
            if(ObjectId.TryParse(stringId, out ObjectId objectId))
                return objectId;
            return null;
        }
    }
}