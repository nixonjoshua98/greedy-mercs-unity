using Newtonsoft.Json;
using System;

namespace GM.Common.Json
{
    public class UnixMillisecondDateTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            long ts = long.Parse(reader.Value.ToString());

            return DateTimeOffset.FromUnixTimeMilliseconds(ts).UtcDateTime;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
