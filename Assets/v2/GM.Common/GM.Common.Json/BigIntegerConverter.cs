using Newtonsoft.Json;
using System;
using System.Numerics;

namespace GM.Common.Json
{
    public class BigIntegerConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(BigInteger);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return BigInteger.Parse(reader.Value.ToString(), System.Globalization.NumberStyles.Any);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
