using System.Collections.Generic;
using System.Linq;

namespace AqoTesting.Core.Utils
{
    public static class FieldsHashGenerator
    {
        public static byte[] Generate(Dictionary<string, string> fields)
        {
            var sortedFields = fields.OrderBy(item => item.Key);

            var stringToHash = "";
            foreach(var field in sortedFields)
                stringToHash += field.Value;

            return Sha256.Compute(stringToHash);
        }
    }
}
