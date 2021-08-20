using System;
using System.IO;
using System.Text;

using UnityEngine;

namespace GM
{
    public static class FileUtils
    {
        public static string ResolvePath(string name) => $"{Application.persistentDataPath}/{name}";

        public static void AppendToTextFile(string path, string value)
        {
            new FileInfo(path).Directory.Create();

            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(value);
            }
        }

        public static string[] LoadTextFileToList(string path)
        {
            return File.ReadAllLines(path);
        }
    }
}
