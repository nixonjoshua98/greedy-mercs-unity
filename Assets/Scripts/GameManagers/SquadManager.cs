using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

using UnityEngine;

namespace GM
{
    using GM.Data;

    using GM.Events;

    using GM.Units;

    public class SquadManager : MonoBehaviour
    {
        public static SquadManager Instance = null;

        List<GameObject> spawnedUnits;

        void Awake()
        {
            Instance = this;

            spawnedUnits = new List<GameObject>();

            GlobalEvents.OnCharacterUnlocked.AddListener(OnHeroUnlocked);
        }


        public List<Vector3> UnitPositions()
        {
            List<Vector3> ls = new List<Vector3>();

            foreach (GameObject o in spawnedUnits)
                ls.Add(o.transform.position);

            return ls;
        }


        public Vector3 AveragePosition()
        {
            return Funcs.AveragePosition(UnitPositions());
        }


        void AddCharacter(MercID mercId)
        {
            Vector3 spawnPos = SpawnPosition();

            MercDescription mercDescription = GameData.Get().Mercs.GetDescription(mercId);

            GameObject character = InstantiateMerc(mercDescription.Prefab, spawnPos);

            spawnedUnits.Add(character);
        }


        Vector3 SpawnPosition()
        {
            Vector3 spawnPos;

            if (spawnedUnits.Count == 0)
            {
                Vector3 temp = Camera.main.transform.position;

                spawnPos = new Vector3(temp.x, 6.0f);
            }
            else
            {
                spawnPos = AveragePosition();
            }

            spawnPos.x -= 2.5f;

            return spawnPos;
        }


        GameObject InstantiateMerc(GameObject prefab, Vector3 pos)
        {
            return Instantiate(prefab, pos, Quaternion.identity);
        }


        void OnHeroUnlocked(MercID chara)
        {
            AddCharacter(chara);
        }
    }
}