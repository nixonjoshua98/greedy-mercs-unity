using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public static class GameObject_Extensions
    {
        public static bool TryGetComponentInChildren<T>(this GameObject obj,  out T component)
        {
            component = obj.GetComponentInChildren<T>();

            return component != null;
        }
    }
}
