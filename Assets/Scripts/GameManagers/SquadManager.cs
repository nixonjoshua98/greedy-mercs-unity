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


        public List<Vector3> UnitPositions() => spawnedUnits.Where(obj => obj != null).Select(obj => obj.transform.position).ToList();


        public Vector3 AveragePosition()
        {
            return Funcs.AveragePosition(UnitPositions());
        }


        void AddCharacter(MercID mercId)
        {
            MercData data = GameData.Get().Mercs.Get(mercId);

            GameObject character = InstantiateAndSetupMerc(data, SpawnPosition());

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


        GameObject InstantiateAndSetupMerc(MercData merc, Vector2 position)
        {
            GameObject o = GameObject.Instantiate(merc.Prefab, position, Quaternion.identity);

            MercController controller = o.GetComponent<MercController>();

            controller.Setup(merc.Id);

            return o;
        }


        void OnHeroUnlocked(MercID chara)
        {
            AddCharacter(chara);
        }
    }
}