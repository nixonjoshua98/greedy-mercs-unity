using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SRC.Common
{
    public enum FileStatus
    {
        OK,
        CORRUPTED,
        NOT_EXISTS
    }

    public static class FileIO
    {
#if UNITY_EDITOR
        private const bool _PlainText = true;
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

        public static bool TryDeserialize<T>(string text, out T result)
        {
            result = default;

            try
            {
                result = JsonConvert.DeserializeObject<T>(text);
            }
            catch
            {

            }

            return result != null;
        }

        public static FileStatus LoadModel<T>(string path, out T result) where T : new()
        {
            result = new T();

            path = ResolvePath(path);

            if (!File.Exists(path))
                return FileStatus.NOT_EXISTS;

            string text = ReadFromFile(path);

            if (TryDeserialize(text, out result))
                return FileStatus.OK;

            result = new T();

            return FileStatus.CORRUPTED;
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
            {
                File.WriteAllText(path, text);
            }
            else
            {
                WriteEncryptedFile(path, text);
            }
        }

        private static void WriteEncryptedFile(string path, string text)
        {
            BinaryFormatter bf = new();

            new FileInfo(path).Directory.Create();

            using FileStream file = File.Open(path, FileMode.OpenOrCreate);

            bf.Serialize(file, AES.Encrypt(text));
        }

        private static string ReadFromFile(string path)
        {
            return _PlainText ? File.ReadAllText(path) : ReadEncryptedFile(path);
        }

        private static string ReadEncryptedFile(string path)
        {
            var bf = new BinaryFormatter();

            using FileStream file = File.Open(path, FileMode.Open);

            string plain = (string)bf.Deserialize(file);

            return AES.Decrypt(plain);
        }
    }
}
