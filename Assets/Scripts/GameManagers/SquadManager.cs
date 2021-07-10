using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

using UnityEngine;

namespace GM
{
    using GM.Events;

    using GM.Units;

    public class SquadManager : MonoBehaviour
    {
        public static SquadManager Instance = null;

        List<Transform> characterSpots;

        List<GameObject> spawnedUnits;

        void Awake()
        {
            Instance = this;

            spawnedUnits = new List<GameObject>();
            characterSpots = new List<Transform>();

            for (int i = 0; i < transform.childCount; ++i)
                characterSpots.Add(transform.GetChild(i));

            GlobalEvents.OnCharacterUnlocked.AddListener(OnHeroUnlocked);
        }

        public List<Vector3> UnitPositions()
        {
            List<Vector3> ls = new List<Vector3>();

            foreach (GameObject o in spawnedUnits)
                ls.Add(o.transform.position);

            return ls;
        }

        void AddCharacter(UnitID unitId)
        {
            MercData data = StaticData.Mercs.GetMerc(unitId);

            GameObject character = Instantiate(data.Prefab, transform);

            character.transform.position = characterSpots[0].position;

            Destroy(characterSpots[0].gameObject);

            characterSpots.RemoveAt(0);

            spawnedUnits.Add(character);
        }

        void OnHeroUnlocked(UnitID chara)
        {
            AddCharacter(chara);
        }
    }
}