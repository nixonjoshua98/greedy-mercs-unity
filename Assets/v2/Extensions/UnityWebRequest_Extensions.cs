using UnityEngine.Networking;

namespace GM
{
    public static class UnityWebRequest_Extensions
    {
        public static bool GetBoolResponseHeader(this UnityWebRequest www, string name, bool defaultValue)
        {
            if (ResponseHeaderExists(www, name))
            {
                return www.GetResponseHeader(name) == "true";
            }

            return defaultValue;
        }

        static bool ResponseHeaderExists(UnityWebRequest www, string name) => www.GetResponseHeaders().ContainsKey(name);
    }
}
