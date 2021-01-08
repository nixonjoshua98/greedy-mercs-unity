using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Numerics;
using System.Collections;
using System.IO.Compression;
using System.Collections.Generic;

using SimpleJSON;

using UnityEngine;

using Vector3 = UnityEngine.Vector3;

public static class Extensions
{    
    public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue fallback)
    {
        return dict.TryGetValue(key, out var value) ? value : fallback;
    }

    public static BigDouble ToBigDouble(this BigInteger val)
    {
        return BigDouble.Parse(val.ToString());
    }

    public static long ToUnixMilliseconds(this DateTime dt)
    {
        return (new DateTimeOffset(dt)).ToUnixTimeMilliseconds();
    }
}

namespace Utils
{
    public class Generic
    {
        public static string BonusToString(BonusType skill)
        {
            switch (skill)
            {
                case BonusType.MERC_DAMAGE:             return "Merc Damage";
                case BonusType.TAP_DAMAGE:              return "Tap Damage";
                case BonusType.HERO_TAP_DAMAGE_ADD:     return "Merc Tap Damage Bonus";
                case BonusType.ENEMY_GOLD:              return "Enemy Gold";
                case BonusType.BOSS_GOLD:               return "Boss Gold";
                case BonusType.MELEE_DAMAGE:            return "Melee Damage";
                case BonusType.MAGE_DAMAGE:             return "Mage Damage";
                case BonusType.CRIT_CHANCE:             return "Critical Hit Chance";
                case BonusType.RANGED_DAMAGE:           return "Ranged Damage";
                case BonusType.ALL_GOLD:                return "All Gold";
                case BonusType.CRIT_DAMAGE:             return "Critical Hit Damage";
                case BonusType.CASH_OUT_BONUS:          return "Cash Out Bonus";

                default:
                    return "<error>";
            }
        }
    }

    public class Lerp
    {
        public static IEnumerator Local(GameObject o, Vector3 start, Vector3 end, float dur)
        {
            float progress = 0.0f;

            o.transform.localPosition = start;

            while (progress < 1.0f)
            {
                progress += (Time.deltaTime / dur);

                o.transform.localPosition = Vector3.Lerp(start, end, progress);

                yield return new WaitForEndOfFrame();
            }
        }
    }


    public class Json
    {
        public static JSONArray CreateJSONArray<TKey, TValue>(string key, Dictionary<TKey, TValue> dict)
        {
            JSONArray array = new JSONArray();

            foreach (var entry in dict)
            {
                JSONNode parsedValue = JSON.Parse(JsonUtility.ToJson(entry.Value));

                parsedValue.Add(key, (int)(object)entry.Key);

                array.Add(parsedValue);
            }

            return array;
        }

        public static JSONNode Decode(string data)
        {
            return JSON.Parse(GZip.Unzip(Convert.FromBase64String(data)));
        }

        public static string Encode(JSONNode node)
        {
            return Convert.ToBase64String(GZip.Zip(node.ToString()));
        }

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

        public static GameObject Instantiate(GameObject o, Transform parent, UnityEngine.Vector3 pos)
        {
            GameObject createdObject = GameObject.Instantiate(o, pos, UnityEngine.Quaternion.identity);

            createdObject.transform.SetParent(parent, false);

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

        public static void Append(string filename, string content)
        {
            string path = GetPath(filename);

            using (StreamWriter w = System.IO.File.AppendText(path))
            {
                w.WriteLine(content);
            }
        }


        public static void WriteJson(string filename, JSONNode node)
        {
            Write(filename, Format.FormatJson(node.ToString()));
        }

        public static bool ReadJson(string filename, out JSONNode node)
        {
            node = null;

            if (Read(filename, out string content))
            {
                node = JSON.Parse(content);

                return true;
            }

            return false;
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
        static Dictionary<int, string> unitsTable = new Dictionary<int, string> { { 0, "" }, { 1, "K" }, { 2, "M" }, { 3, "B" }, { 4, "T" } };

        public static string FormatNumber(BigInteger val)
        {
            if (val <= 1_000)
                return val.ToString();

            int n = (int)BigInteger.Log(val, 1000);

            BigDouble m = val.ToBigDouble() / BigInteger.Pow(1000, n).ToBigDouble();

            if (n < unitsTable.Count)
                return m.ToString("F2") + unitsTable[n];

            return val.ToString("E2").Replace("+", "").Replace("E", "e");
        }

        public static string FormatNumber(BigDouble val)
        {
            if (val < 1d)
                return val.ToString("F0");
            
            int n = (int)BigDouble.Log(val, 1000);

            BigDouble m = val / BigDouble.Pow(1000.0f, n);

            if (n < unitsTable.Count)
                return (BigDouble.Floor(m * 100.0f) / 100.0f).ToString("F2") + unitsTable[n];

            return val.ToString("E2").Replace("+", "").Replace("E", "e");
        }

        public static string FormatNumber(double val)
        {
            if (val < 1d)
                return Math.Round(val, 3).ToString();

            int n = (int)Math.Log(val, 1000);

            float m = (float)(val / Mathf.Pow(1000.0f, n));

            if (n < unitsTable.Count)
                return m.ToString("F") + unitsTable[n];
            
            return val.ToString("e2").Replace("+", "");
        }

        public static string FormatJson(string json, string indent="  ")
        {
            var indentation = 0;
            var quoteCount = 0;
            var escapeCount = 0;

            var result =
                from ch in json ?? string.Empty
                let escaped = (ch == '\\' ? escapeCount++ : escapeCount > 0 ? escapeCount-- : escapeCount) > 0
                let quotes = ch == '"' && !escaped ? quoteCount++ : quoteCount
                let unquoted = quotes % 2 == 0
                let colon = ch == ':' && unquoted ? ": " : null
                let nospace = char.IsWhiteSpace(ch) && unquoted ? string.Empty : null
                let lineBreak = ch == ',' && unquoted ? ch + Environment.NewLine + string.Concat(Enumerable.Repeat(indent, indentation)) : null
                let openChar = (ch == '{' || ch == '[') && unquoted ? ch + Environment.NewLine + string.Concat(Enumerable.Repeat(indent, ++indentation)) : ch.ToString()
                let closeChar = (ch == '}' || ch == ']') && unquoted ? Environment.NewLine + string.Concat(Enumerable.Repeat(indent, --indentation)) + ch : ch.ToString()
                select colon ?? nospace ?? lineBreak ?? (
                    openChar.Length > 1 ? openChar : closeChar
                );

            return string.Concat(result);
        }

        public static string FormatSeconds(int seconds)
        {
            int hours = seconds / 3_600; seconds -= (3_600 * hours);
            int mins = seconds / 60; seconds -= (60 * mins);

            return string.Format("{0}:{1}:{2}", hours.ToString().PadLeft(2, '0'), mins.ToString().PadLeft(2, '0'), seconds.ToString().PadLeft(2, '0'));
        }
    }
}