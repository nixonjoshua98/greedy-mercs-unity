using UnityEngine;
using SRC.Common;
using System.Collections.Generic;

namespace SRC
{
    public static class TransformExtensions
    {
        public static void DestroyChildren(this Transform transform)
        {
            var children = new List<GameObject>();

            for (int i = 0; i < transform.childCount; i++)
            {
                children.Add(transform.GetChild(i).gameObject);
            }

            children.ForEach(child => Object.Destroy(child));
        }

        public static void AddPositionVector(this Transform transform, Vector3 position)
        {
            transform.position = new Vector3(
                transform.position.x + position.x,
                transform.position.y + position.y,
                transform.position.z + position.z);
        }

        public static void ShuffleChildren(this Transform trans, object? seed = null)
        {
            seed ??= trans.GetInstanceID();

            var rnd = Utility.SeededRandom(seed);

            for (int i = 0; i < trans.childCount; i++)
            {
                var child = trans.GetChild(i);

                child.SetSiblingIndex(rnd.Next(0, trans.childCount));
            }
        }
    }
}
