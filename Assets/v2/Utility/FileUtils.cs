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
        public static string ResolvePath(string name) => $"{Application.persistentDataPath}/{name}";

        public static void DeleteFile(string path) => File.Delete(ResolvePath(path));

        public static FileStatus LoadModel<T>(string path, out T result) where T: new()
        {
            result = new T();

            path = ResolvePath(path);

            try
            {
                if (!File.Exists(path))
                    return FileStatus.NOT_EXISTS;

                result = JsonConvert.DeserializeObject<T>(AES.Decrypt(ReadFromFile(path)));
                //result = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));

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

            //File.WriteAllText(path, JsonConvert.SerializeObject(model));

            string json = JsonConvert.SerializeObject(model);
            string encrypted = AES.Encrypt(json);

            WriteToFile(path, encrypted);
        }

        static void WriteToFile(string path, string text)
        {
            BinaryFormatter bf = new BinaryFormatter();

            new FileInfo(path).Directory.Create();

            using (FileStream file = File.Open(path, FileMode.OpenOrCreate))
            {
                bf.Serialize(file, text);
            }
        }

        static string ReadFromFile(string path)
        {
            BinaryFormatter bf = new BinaryFormatter();

            using (FileStream file = File.Open(path, FileMode.Open))
            {
                return (string)bf.Deserialize(file);
            }
        }
    }
}
