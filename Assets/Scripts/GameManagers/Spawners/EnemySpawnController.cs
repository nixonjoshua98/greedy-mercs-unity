using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM
{
    using GM.Formations;

    public class EnemySpawnController : MonoBehaviour
    {
        [SerializeField] GameObject[] EnemyObjects;

        [SerializeField] UnitFormation[] unitFormations;

        public List<GameObject> SpawnMultiple()
        {
            Vector3 centerPos = GetCenterPosition();

            List<GameObject> spawnedObjects = new List<GameObject>();

            UnitFormation formation = unitFormations[Random.Range(0, unitFormations.Length)];

            for (int i = 0; i < formation.numPositions; ++i)
            {
                Vector3 spawnPos = centerPos + formation.GetPosition(i);

                spawnedObjects.Add(Spawn(spawnPos));
            }

            return spawnedObjects;
        }

        // Calculate the center point for the formation, the actual position used
        // will be the center point returned from here + the relative position from
        // the current formation
        Vector3 GetCenterPosition()
        {
            Vector3 pos = Camera.main.MaxBounds();

            return new Vector3(pos.x, 5.5f);
        }

        GameObject Spawn(Vector3 pos)
        {
            return Instantiate(EnemyObjects[Random.Range(0, EnemyObjects.Length)], pos, Quaternion.identity);
        }
    }
}