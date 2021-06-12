using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM
{
    public class EnemySpawnController : MonoBehaviour
    {
        [SerializeField] GameObject[] EnemyObjects;

        public List<GameObject> SpawnMultiple(int n)
        {
            List<GameObject> ls = new List<GameObject>();

            for (int i = 0; i < n; ++i)
            {
                ls.Add(Spawn(new Vector3(Random.Range(-3, 3), Random.Range(5, 2))));
            }

            return ls;

        }

        GameObject Spawn(Vector3 pos)
        {
            GameObject o = Instantiate(EnemyObjects[Random.Range(0, EnemyObjects.Length)], pos, Quaternion.identity);

            return o;
        }
    }
}