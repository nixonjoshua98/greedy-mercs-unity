using Newtonsoft.Json;
using System;
using UnityEngine;

namespace GM.Common.Json
{
    public class UnixMillisecondUTCDateTimeConverter : JsonConverter
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
