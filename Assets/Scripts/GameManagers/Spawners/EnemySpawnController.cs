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

            Vector3 pos = new Vector3(3.6f, 4.6f);

            for (int i = 0; i < n; ++i)
            {
                ls.Add(Spawn(pos));

                pos.y -= 1;
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