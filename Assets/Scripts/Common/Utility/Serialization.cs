using Newtonsoft.Json;
using System;

namespace SRC.Common
{
    public static class Serialization
    {
        public static bool TryDeserialize<T>(string text, out T model)
        {
            model = default;

            try
            {
                model = JsonConvert.DeserializeObject<T>(text);
            }
            catch (Exception ex)
            {
                return false;
            }

            return model is not null;
        }
    }
}
