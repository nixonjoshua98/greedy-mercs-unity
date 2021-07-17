using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public class CanvasUtils
    {
        static GameObject MainCanvas { get { return GameObject.FindGameObjectWithTag("MainCanvas"); } }

        public static GameObject Instantiate(GameObject o, Vector3 pos)
        {
            GameObject createdObject = GameObject.Instantiate(o);

            createdObject.transform.SetParent(MainCanvas.transform, false);

            createdObject.transform.position = pos;

            return createdObject;
        }
    }
}
