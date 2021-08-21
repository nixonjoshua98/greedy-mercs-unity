using SimpleJSON;
using System.IO;
using UnityEngine;

namespace GM
{
    public static class FileUtils
    {
        public static string ResolvePath(string name) => $"{Application.persistentDataPath}/{name}";

        public static string LoadJSON(string path)
        {
            return File.Exists(path) ? AES.Decrypt(path) : string.Empty;
        }


        public static bool OpenJSON(string path, out JSONNode result)
        {
            result = new JSONObject();

            if (!File.Exists(path))
                return false;

            string contents = AES.Decrypt(path);

            // SimpleJSON cannot parse extra " or \ which is added when saving to file for some reason (bug) but Newtonsoft
            // can parse it and remove it so we can do extra cast to Newtonsoft then to SimpleJSON
            result = JSON.Parse(Newtonsoft.Json.JsonConvert.DeserializeObject(contents).ToString());

            return true;
        }


        public static void WriteJSON(string path, JSONNode node)
        {
            new FileInfo(path).Directory.Create();

            AES.Encrypt(path, node.ToString());
        }
    }
}
