using System;
using System.IO;
using System.Text;
using System.Collections;
using System.IO.Compression;
using System.Collections.Generic;

using UnityEngine;

namespace Utils
{
    public class UI
    {
        public static GameObject Instantiate(GameObject o, Vector3 pos)
        {
            GameObject canvas = GameObject.FindGameObjectWithTag("MainCanvas");

            GameObject createdObject = GameObject.Instantiate(o, pos, Quaternion.identity);

            createdObject.transform.SetParent(canvas.transform, false);

            return createdObject;
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
            var bytes = Encoding.UTF8.GetBytes(str);

            return Zip(bytes);
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

    }

    public class Format : MonoBehaviour
    {
        public static string DoubleToString(double val)
        {
            if (val < 1d)
                return Math.Round(val, 1).ToString();

            Dictionary<int, string> unitsTable = new Dictionary<int, string> { { 0, "" }, { 1, "K" }, { 2, "M" }, { 3, "B" }, { 4, "T" } };

            int n = (int)Math.Log(val, 1000);

            float m = (float)(val / Mathf.Pow(1000.0f, n));

            if (n < unitsTable.Count)
                return (Mathf.Floor(m * 100.0f) / 100.0f).ToString("0.#") + unitsTable[n];

            else
                return val.ToString("e2").Replace("+", "");
        }
    }
}