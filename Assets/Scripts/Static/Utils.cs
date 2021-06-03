using System;
using System.IO;
using System.Text;
using System.Numerics;
using System.Collections;
using System.IO.Compression;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

using Vector3 = UnityEngine.Vector3;

public static class DictionaryExtensions
{
    public static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue fallback)
    {
        return dict.TryGetValue(key, out var value) ? value : fallback;
    }
}

public static class Extensions
{

    public static BigDouble ToBigDouble(this BigInteger val)
    {
        return BigDouble.Parse(val.ToString());
    }

    public static BigInteger ToBigInteger(this BigDouble val)
    {
        return BigInteger.Parse(val.Ceiling().ToString("F0"));
    }

    public static DateTime ToUnixDatetime(this long val)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(val).DateTime;
    }
}

public class StringFormatting : MonoBehaviour
{
    static Dictionary<int, string> unitsTable = new Dictionary<int, string> { { 0, "" }, { 1, "K" }, { 2, "M" }, { 3, "B" }, { 4, "T" } };

    public static string Number(double val)
    {
        if (val < 1d)
            return Math.Round(val, 3).ToString();

        int n = (int)Math.Log(val, 1000);

        float m = (float)(val / Mathf.Pow(1000.0f, n));

        if (n < unitsTable.Count)
            return m.ToString("F") + unitsTable[n];

        return val.ToString("e2").Replace("+", "");
    }
}

    public static class Funcs
{
    public static string BonusString(BonusType bonusType)
    {
        return bonusType.ToString();
    }

    public static string BonusString(BonusType bonusType, double val)
    {
        return string.Format("{0} {1}", val, BonusString(bonusType));
    }

    // = = = Time = = = //
    public static DateTime ToDateTime(long timestamp) => DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;
    public static TimeSpan TimeUntil(DateTime date) => date - DateTime.UtcNow;

    public static JSONNode DecryptServerJSON(string data)
    {
        return JSON.Parse(GM.Utils.GZip.Unzip(Convert.FromBase64String(data)));
    }

    public static class Format
    {
        public static string Seconds(double seconds) { return Seconds((long)seconds); }
        public static string Seconds(long seconds)
        {
            long hours = seconds / 3_600;            
            seconds -= (3_600 * hours);

            long mins = seconds / 60;
            seconds -= (60 * mins);

            return string.Format("{0}:{1}:{2}", hours.ToString().PadLeft(2, '0'), mins.ToString().PadLeft(2, '0'), seconds.ToString().PadLeft(2, '0'));
        }
    }

    public static class UI
    {
        public static GameObject Instantiate(GameObject o, Transform parent) { return Instantiate(o, parent.gameObject); }

        public static GameObject Instantiate(GameObject o, Transform parent, Vector3 pos)
        {
            GameObject createdObject = GameObject.Instantiate(o, pos, UnityEngine.Quaternion.identity);

            createdObject.transform.SetParent(parent, false);

            return createdObject;
        }

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

namespace GM.Utils
{
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

            rt.localScale = Vector3.one;
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
            return Funcs.DecryptServerJSON(data);
        }

        public static JSONNode GetDeviceInfo()
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
    }
}