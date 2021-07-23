using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public class CanvasUtils
    {
        public static GameObject MainCanvas => GameObject.FindGameObjectWithTag("MainCanvas");

        // = = = Instantiate = = = //
        public static GameObject Instantiate(GameObject o) => Instantiate(o, MainCanvas);
        public static GameObject Instantiate(GameObject o, Transform parent) => Instantiate(o, parent.gameObject);

        public static GameObject Instantiate(GameObject o, GameObject parent)
        {
            GameObject inst = GameObject.Instantiate(o);

            inst.transform.SetParent(parent.transform, false);

            return inst;
        }


        public static GameObject Instantiate(GameObject o, Vector3 pos)
        {
            GameObject inst = Instantiate(o, MainCanvas);

            inst.transform.position = pos;

            return inst;
        }


        // = = = Popups = = = //
        public static void ShowInfo(string title, string desc)
        {
            GameObject o = Resources.Load<GameObject>("Popups/Info");

            Message msg = Instantiate(o).GetComponent<Message>();

            msg.Init(title.ToUpper(), desc);
        }
    }
}
