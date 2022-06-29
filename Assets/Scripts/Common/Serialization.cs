using Newtonsoft.Json;
using System;

namespace GM.Common
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
                GMLogger.Exception("DeserializeObject", ex);

                return false;
            }

            return model is not null;
        }
    }
}
