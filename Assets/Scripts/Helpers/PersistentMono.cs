using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreedyMercs
{
    public class PersistentMono : MonoBehaviour
    {
        static PersistentMono _instance = null;

        public static PersistentMono Inst
        {
            get
            {
                if (_instance == null)
                {
                    GameObject o = new GameObject("PersistentMono");

                    _instance = o.AddComponent<PersistentMono>();

                    DontDestroyOnLoad(o);
                }

                return _instance;
            }
        }
    }
}