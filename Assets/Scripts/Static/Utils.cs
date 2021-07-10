using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

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
}