using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace GM
{
    public enum FileStatus
    {
        OK,
        CORRUPTED,
        NOT_EXISTS
    }

    public static class FileUtils
    {
#if UNITY_EDITOR
        const bool _PlainText = true;
#else
        const bool _PlainText = false;
#endif

        public static string ResolvePath(string name)
        {
            return $"{Application.persistentDataPath}/{name}";
        }

        public static void DeleteFile(string path)
        {
            File.Delete(ResolvePath(path));
        }

        public static FileStatus LoadModel<T>(string path, out T result) where T : new()
        {
            result = new T();

            path = ResolvePath(path);

            try
            {
                if (!File.Exists(path))
                    return FileStatus.NOT_EXISTS;

                result = JsonConvert.DeserializeObject<T>(ReadFromFile(path));

                if (result == null)
                {
                    result = new T();
                    return FileStatus.CORRUPTED;
                }

                return FileStatus.OK;
            }
            catch
            {
                result = new T();
                return FileStatus.CORRUPTED;
            }
        }

        public static void WriteModel<T>(string path, T model)
        {
            path = ResolvePath(path);

            string json = JsonConvert.SerializeObject(model);

            WriteToFile(path, json);
        }

        private static void WriteToFile(string path, string text)
        {
            if (_PlainText)
                File.WriteAllText(path, text);

            else
            {
                BinaryFormatter bf = new();

                new FileInfo(path).Directory.Create();

                using (FileStream file = File.Open(path, FileMode.OpenOrCreate))
                {
                    bf.Serialize(file, AES.Encrypt(text));
                }
            }
        }

        private static string ReadFromFile(string path)
        {
            if (_PlainText)
                return File.ReadAllText(path);

            BinaryFormatter bf = new();

            using (FileStream file = File.Open(path, FileMode.Open))
            {
                string plain = (string)bf.Deserialize(file);

                return AES.Decrypt(plain);
            }
        }
    }
}
