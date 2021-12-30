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

            path = ResolvePath(path);

            if (!File.Exists(path))
                return false;

            string contents = AES.Decrypt(File.ReadAllText(path));

            result = JSON.Parse(contents);

            return true;
        }


        public static void WriteJSON(string path, JSONNode node)
        {
            path = ResolvePath(path);

            if (File.Exists(path))
                File.Delete(path);

            new FileInfo(path).Directory.Create();

            File.WriteAllText(path, AES.Encrypt(node.ToString()));
        }
    }
}
