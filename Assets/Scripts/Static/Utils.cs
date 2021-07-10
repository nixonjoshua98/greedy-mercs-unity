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


public static class Funcs
{
    public static Vector3 AveragePosition(List<Vector3> ls)
    {
        Vector3 avg = Vector3.zero;

        foreach (Vector3 pos in ls)
            avg += pos;

        return avg / ls.Count;
    }


    public static string BonusString(BonusType bonusType)
    {
        return bonusType.ToString();
    }

    // = = = Time = = = //
    public static DateTime ToDateTime(long ts)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(ts).UtcDateTime;
    }

    public static TimeSpan TimeUntil(DateTime date)
    {
        return date - DateTime.UtcNow;
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

    public class Format : MonoBehaviour
    {
        static readonly Dictionary<int, string> unitsTable = new Dictionary<int, string> { { 0, "" }, { 1, "K" }, { 2, "M" }, { 3, "B" }, { 4, "T" } };

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
    }
}