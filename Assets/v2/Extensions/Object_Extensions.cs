using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Diagnostics;
using UnityEngine.SceneManagement;

namespace GM
{
    public static class Object_Extensions
    {
        public static T GetComponentInScene<T>(this MonoBehaviour @this)
        {
            Stopwatch sw = Stopwatch.StartNew();

            GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            try
            {
                foreach (var rootGameObject in rootGameObjects)
                {
                    T component = rootGameObject.GetComponentInChildren<T>();

                    if (component != null)
                    {
                        return component;
                    }
                }

                return default(T);
            }

            finally
            {
                GMLogger.Editor($"GetComponentInScene() | {sw.ElapsedMilliseconds}ms ");
            }
        }
    }
}