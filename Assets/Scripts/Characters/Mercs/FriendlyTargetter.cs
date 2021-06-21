using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Targetting
{
    public class FriendlyTargetter : MonoBehaviour, IAttackTarget
    {
        public TargetPriority targetPriority = TargetPriority.RANDOM;

        string targetTag = "Enemy";

        public GameObject GetTarget()
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(targetTag);

            if (objects.Length > 0)
                return PullTarget(objects);

            return null;
        }

        GameObject PullTarget(GameObject[] objects)
        {
            switch (targetPriority)
            {
                case TargetPriority.FIRST:
                    return objects[0];

                case TargetPriority.LAST:
                    return objects[objects.Length - 1];

                case TargetPriority.RANDOM:
                    return objects[Random.Range(0, objects.Length)];

                default:
                    return objects[0];
            }
        }
    }
}