using SimpleJSON;
using System.IO;
using UnityEngine;

namespace GM
{
    public static class FileUtils
    {
        public static string ResolvePath(string name) => $"{Application.persistentDataPath}/{name}";

        public static bool LoadJSON(string path, out JSONNode result)
        {
            result = new JSONObject();

            if (!File.Exists(path))
                return false;

            string contents = AES.Decrypt(path);

            result = JSON.Parse(contents);

            return true;
        }


        public static void WriteJSON(string path, JSONNode node)
        {
            new FileInfo(path).Directory.Create();

            AES.Encrypt(path, node.ToString());
        }
    }
}
