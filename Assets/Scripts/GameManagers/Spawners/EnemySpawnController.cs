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
            List<Vector3> unitPositions = SquadManager.Instance.UnitPositions();

            List<GameObject> ls = new List<GameObject>();

            Vector3 pos = new Vector3(2.5f, 7.4f);

            if (unitPositions.Count >= 1)
            {
                Vector3 averageUnitPosition = Funcs.AveragePosition(unitPositions);

                pos.x = averageUnitPosition.x + 2.0f;
            }


            for (int i = 0; i < n; ++i)
            {
                ls.Add(Spawn(pos));
                
                pos.y -= 1.5f;
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