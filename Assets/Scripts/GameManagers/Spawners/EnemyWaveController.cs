using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM
{
    public class EnemyWaveController : MonoBehaviour
    {
        [SerializeField] Transform SpawnPoint;
        [Space]
        [SerializeField]
        GameObject[] EnemyObjects;

        public GameObject Spawn()
        {
            return SpawnEnemy();
        }

        GameObject SpawnEnemy()
        {
            GameObject o = Instantiate(EnemyObjects[Random.Range(0, EnemyObjects.Length)], SpawnPoint.position, Quaternion.identity);

            return o;
        }
    }
}