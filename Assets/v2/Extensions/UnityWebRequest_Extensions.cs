using UnityEngine.Networking;

namespace GM
{
    public static class UnityWebRequest_Extensions
    {
        public static bool GetBoolResponseHeader(this UnityWebRequest www, string name, bool defaultValue)
        {
            return ResponseHeaderExists(www, name) ? www.GetResponseHeader(name) == "true" : defaultValue;
        }

        static bool ResponseHeaderExists(UnityWebRequest www, string name)
        {
            return (www.GetResponseHeaders() ?? new()).ContainsKey(name);
        }
    }
}
