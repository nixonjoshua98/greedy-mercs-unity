using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Collections;
using System.IO.Compression;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using SimpleJSON;

using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

using SysFile = System.IO.File;

public static class Funcs
{
    public static JSONNode SerializeDictionary<TKey, TVal>(Dictionary<TKey, TVal> dict)
    {
        JSONNode node = new JSONObject();

        foreach (KeyValuePair<TKey, TVal> entry in dict)
        {
            node.Add(entry.Key.ToString(), JSON.Parse(JsonUtility.ToJson(entry.Value)));
        }

        return node;
    }

    public static class UI
    {
        public static GameObject MainCanvas { get { return GameObject.FindGameObjectWithTag("MainCanvas"); } }
        public static GameObject Instantiate(GameObject obj, GameObject parent = null)
        {
            if (parent == null)
                parent = MainCanvas;

            GameObject createdObject = GameObject.Instantiate(obj);

            createdObject.transform.SetParent(parent.transform, false);

            return createdObject;
        }
    }

}


public static class Extensions
{    
    public static TValue GetOrVal<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue fallback)
    {
        return dict.TryGetValue(key, out var value) ? value : fallback;
    }

    public static BigDouble ToBigDouble(this BigInteger val)
    {
        return BigDouble.Parse(val.ToString());
    }

    public static BigInteger ToBigInteger(this BigDouble val)
    {
        return BigInteger.Parse(val.Ceiling().ToString("F0"));
    }

    public static long ToUnixMilliseconds(this DateTime dt)
    {
        return (new DateTimeOffset(dt)).ToUnixTimeMilliseconds();
    }

    public static DateTime ToUnixDatetime(this long val)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(val).DateTime;
    }
}

namespace GreedyMercs.Utils
{
    using GreedyMercs.UI.Messages;

    public class Generic
    {
        public static string BonusToString(BonusType skill)
        {
            switch (skill)
            {
                case BonusType.MERC_DAMAGE:         return "Merc Damage";
                case BonusType.TAP_DAMAGE:          return "Tap Damage";
                case BonusType.CHAR_TAP_DAMAGE_ADD: return "Bonus to Tap Damage";
                case BonusType.ENEMY_GOLD:          return "Enemy Gold";
                case BonusType.BOSS_GOLD:           return "Boss Gold";
                case BonusType.MELEE_DAMAGE:        return "Melee Damage";
                case BonusType.CRIT_CHANCE:         return "Critical Hit Chance";
                case BonusType.RANGED_DAMAGE:       return "Ranged Damage";
                case BonusType.ALL_GOLD:            return "All Gold";
                case BonusType.CRIT_DAMAGE:         return "Critical Hit Damage";
                case BonusType.CASH_OUT_BONUS:      return "Cash Out Bonus";
                case BonusType.ENERGY_INCOME:       return "Energy Income";
                case BonusType.ENERGY_CAPACITY:     return "Energy Capacity";
                case BonusType.GOLD_RUSH_BONUS:     return "Gold Rush Bonus";
                case BonusType.GOLD_RUSH_DURATION:  return "Gold Rush Duration";
                case BonusType.AUTO_CLICK_BONUS:    return "Auto Click Damage";
                case BonusType.AUTO_CLICK_DURATION: return "Auto Click Duration";
                case BonusType.BOSS_TIMER_DURATION: return "Boss Timer Duration";

                default:
                    return "<missing name>";
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

        public static IEnumerator RectTransform(RectTransform rt, Vector3 start, Vector3 end, float dur)
        {
            float progress = 0.0f;

            rt.localScale = start;

            while (progress < 1.0f)
            {
                progress += (Time.deltaTime / dur);

                rt.localScale = Vector3.Lerp(start, end, progress);

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

        public static JSONNode Decompress(string data)
        {
            return JSON.Parse(File.Decompress(data));
        }

        public static string Compress(JSONNode node)
        {
            return File.Compress(node.ToString());
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
        public static GameObject Instantiate(GameObject o, Vector3 pos)
        {
            GameObject canvas = GameObject.FindGameObjectWithTag("MainCanvas");

            GameObject createdObject = GameObject.Instantiate(o, pos, UnityEngine.Quaternion.identity);

            createdObject.transform.SetParent(canvas.transform, false);

            return createdObject;
        }

        public static GameObject Instantiate(GameObject o, Transform parent, Vector3 pos)
        {
            GameObject createdObject = GameObject.Instantiate(o, pos, UnityEngine.Quaternion.identity);

            createdObject.transform.SetParent(parent, false);

            return createdObject;
        }

        public static void ShowMessage(string title, string desc)
        {
            GameObject o = Resources.Load<GameObject>("Message");

            Message msg = Instantiate(o, Vector3.zero).GetComponent<Message>();

            msg.Init(title, desc);
        }

        public static void ShowMessage(string name, string title, string desc)
        {
            GameObject o = Resources.Load<GameObject>(name);

            Message msg = Instantiate(o, Vector3.zero).GetComponent<Message>();

            msg.Init(title, desc);
        }

        public static void ShowYesNoPrompt(string title, UnityAction callback)
        {
            GameObject o = Resources.Load<GameObject>("YesNoPrompt");

            YesNoPrompt prompt = Instantiate(o, Vector3.zero).GetComponent<YesNoPrompt>();

            prompt.Init(title, "", callback);
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

        public static string Compress(string data) =>  Convert.ToBase64String(GZip.Zip(data));
        public static string Decompress(string data) => GZip.Unzip(Convert.FromBase64String(data));

        public static void SecureWrite(string filename, string obj)
        {
            BinaryFormatter bf = new BinaryFormatter();

            using (FileStream file = SysFile.Open(GetPath(filename), FileMode.OpenOrCreate))
            {
                bf.Serialize(file, Compress(obj));
            }
        }

        public static bool SecureRead(string filename, out string result)
        {
            result = default;

            string path = GetPath(filename);

            if (SysFile.Exists(path))
            {
                BinaryFormatter bf = new BinaryFormatter();

                using (FileStream file = SysFile.Open(GetPath(filename), FileMode.Open))
                {
                    result = Decompress((string)bf.Deserialize(file));

                    return true;
                }
            }

            return false;
        }

        public static bool Read(string filename, out string content)
        {
            string path = GetPath(filename);

            content = "";

            if (SysFile.Exists(path))
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


        public static void WriteJson(string filename, JSONNode node)
        {
            Write(filename, node.ToString());
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

        public static string FormatNumber(long val)
        {
            if (val <= 1_000)
                return val.ToString();

            int n = (int)Mathf.Log(val, 1000);

            double m = val / Mathf.Pow(1000, n);

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

        public static string FormatSeconds(int seconds)
        {
            int hours = seconds / 3_600; seconds -= (3_600 * hours);
            int mins = seconds / 60; seconds -= (60 * mins);

            return string.Format("{0}:{1}:{2}", hours.ToString().PadLeft(2, '0'), mins.ToString().PadLeft(2, '0'), seconds.ToString().PadLeft(2, '0'));
        }
    }
}