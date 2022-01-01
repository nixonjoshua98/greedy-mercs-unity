using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

namespace GM.Common
{
    public abstract class MonoClass<T> : Core.GMMonoBehaviour where T : MonoBehaviour
    {
        static T Instance;

        public static T Create()
        {
            if (Instance == null)
            {
                GameObject o = new GameObject(typeof(T).FullName);

                Instance = o.AddComponent<T>();

                DontDestroyOnLoad(o);
            }

            return Instance;
        }
    }
}
