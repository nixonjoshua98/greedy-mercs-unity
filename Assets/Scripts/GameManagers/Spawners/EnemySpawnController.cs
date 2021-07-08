using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM
{
    using GM.Formations;

    public class EnemySpawnController : MonoBehaviour
    {
        [SerializeField] GameObject[] EnemyObjects;

        [SerializeField] StandardUnitFormation unitFormation;

        public List<GameObject> SpawnMultiple(int n)
        {
            Vector3 centerPos = GetCenterPosition();

            List<GameObject> spawnedObjects = new List<GameObject>();

            for (int i = 0; i < n; ++i)
            {
                Vector3 spawnPos = centerPos;

                if (i < unitFormation.numPositions)
                    spawnPos += unitFormation.GetPosition(i);

                spawnedObjects.Add(Spawn(spawnPos));
            }

            return spawnedObjects;
        }

        // Calculate the center point for the formation, the actual position used
        // will be the center point returned from here + the relative position from
        // the current formation
        Vector3 GetCenterPosition()
        {
            List<Vector3> positions = SquadManager.Instance.UnitPositions();

            if (positions.Count == 0)
                return new Vector3(0.0f, 5.5f);

            Vector3 centerPos = Funcs.AveragePosition(positions);

            return new Vector3(centerPos.x + 5.0f, 5.5f);
        }

        GameObject Spawn(Vector3 pos)
        {
            return Instantiate(EnemyObjects[Random.Range(0, EnemyObjects.Length)], pos, Quaternion.identity);
        }
    }
}