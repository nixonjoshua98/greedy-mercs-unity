using UnityEngine;

namespace GM
{
    public static class Vector_Extensions
    {
        public static Vector3 ToVector3(this Vector2 v2) => new Vector3(v2.x, v2.y);
    }
}