using System;
using System.IO;
using System.Text;
using System.Numerics;
using System.IO.Compression;
using System.Collections.Generic;

using SimpleJSON;

using UnityEngine;

public static class Extensions
{    public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue fallback)
    {
        return dict.TryGetValue(key, out var value) ? value : fallback;
    }
}

namespace Utils
{
    public class Json
    {
        public static JSONNode GetDeviceNode()
        {
            JSONObject node = new JSONObject();

            node.Add("deviceId", SystemInfo.deviceUniqueIdentifier);

            return node;
        }
    }

    public class UI
    {
        public static GameObject Instantiate(GameObject o, UnityEngine.Vector3 pos)
        {
            GameObject canvas = GameObject.FindGameObjectWithTag("MainCanvas");

            GameObject createdObject = GameObject.Instantiate(o, pos, UnityEngine.Quaternion.identity);

            createdObject.transform.SetParent(canvas.transform, false);

            return createdObject;
        }

        public static void ShowError(GameObject o, string title, string desc)
        {
            if (Instantiate(o, UnityEngine.Vector3.zero).TryGetComponent(out ErrorMessage error))
            {
                error.Title.text = title;
                error.Description.text = desc;
            }
        }
    }
    public class GZip
    {
        // Given by William at Tier 9 Studios (Auto Battles Online)

        public static byte[] Zip(byte[] bytes)
        {
            byte[] zippedBytes = null;

            using (MemoryStream msi = new MemoryStream(bytes))
            {
                using (MemoryStream mso = new MemoryStream())
                {
                    using (var gs = new GZipStream(mso, CompressionMode.Compress))
                    {
                        msi.CopyTo(gs);
                    }

                    zippedBytes = mso.ToArray();

                }
            }
            return zippedBytes;
        }

        public static byte[] Zip(string str)
        {
            return Zip(Encoding.UTF8.GetBytes(str));
        }

        public static string Unzip(byte[] bytes)
        {
            string unzippedStr = null;

            using (MemoryStream msi = new MemoryStream(bytes))
            {
                using (MemoryStream mso = new MemoryStream())
                {
                    using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                    {
                        gs.CopyTo(mso);
                    }
                    unzippedStr = Encoding.UTF8.GetString(mso.ToArray());
                }
            }

            return unzippedStr;
        }
    }

    public class File
    {
        static string GetPath(string filename) { return Application.persistentDataPath + "/" + filename; }

        public static bool Read(string filename, out string content)
        {
            string path = GetPath(filename);

            content = "";

            if (System.IO.File.Exists(path))
            {
                using (StreamReader file = new StreamReader(GetPath(filename)))
                {
                    content = file.ReadToEnd();

                    return true;
                }
            }

            return false;
        }

        public static void Write(string filename, string content)
        {
            using (StreamWriter file = new StreamWriter(GetPath(filename)))
            {
                file.Write(content);
            }
        }

        public static void Delete(string filename)
        {
            string path = GetPath(filename);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

    }

    public class Format : MonoBehaviour
    {
        public static string FormatNumber(ulong val)
        {
            if (val < 1000)
                return val.ToString();

            Dictionary<int, string> unitsTable = new Dictionary<int, string> { { 0, "" }, { 1, "K" }, { 2, "M" }, { 3, "B" }, { 4, "T" } };

            int n = (int)Mathf.Log(val, 1000);           

            float m = (float)(val * 1.0f / (int)Mathf.Pow(1000.0f, n));

            if (n < unitsTable.Count)
                return (Mathf.Floor(m * 100.0f) / 100.0f).ToString() + unitsTable[n];

            return val.ToString("e2").Replace("+", "");
        }

        public static string FormatNumber(double val)
        {
            if (val < 1d)
                return Math.Round(val, 2).ToString();

            Dictionary<int, string> unitsTable = new Dictionary<int, string> { { 0, "" }, { 1, "K" }, { 2, "M" }, { 3, "B" }, { 4, "T" } };

            int n = (int)Math.Log(val, 1000);

            float m = (float)(val / Mathf.Pow(1000.0f, n));

            if (n < unitsTable.Count)
                return (Mathf.Floor(m * 100.0f) / 100.0f).ToString("0.##") + unitsTable[n];
            
            return val.ToString("e2").Replace("+", "");
        }
    }
}