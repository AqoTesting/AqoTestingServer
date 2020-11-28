using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace AqoTesting.Core.Utils
{
    public class DateTimeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            return DateTime.Parse(token.ToObject<string>()!);
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(DateTime));
        }
    }
}
