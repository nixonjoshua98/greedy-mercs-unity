using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    using GM.Events;

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
            GameObject o = Instantiate(EnemyObjects[Random.Range(0, EnemyObjects.Length)], SpawnPoint.position, Quaternion.identity, SpawnPoint);

            return o;
        }
    }
}