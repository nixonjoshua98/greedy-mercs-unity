using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public class CanvasUtils
    {
        public static GameObject MainCanvas => GameObject.FindGameObjectWithTag("MainCanvas");

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
    }
}
