using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Targetting
{
    public class FriendlyTargetter : MonoBehaviour, IAttackTarget
    {
        string targetTag = "Enemy";

        public GameObject GetTarget()
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(targetTag);

            if (objects.Length > 0)
                return objects[Random.Range(0, objects.Length)];

            return null;
        }
    }
}