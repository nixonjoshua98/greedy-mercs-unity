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

    struct SpawnedUnit
    {
        public MercID ID;
        public GameObject Object;
    }



    public class SquadManager : MonoBehaviour
    {
        public static SquadManager Instance = null;

        List<SpawnedUnit> mercs;

        void Awake()
        {
            Instance = this;

            mercs = new List<SpawnedUnit>();

            GlobalEvents.E_OnMercUnlocked.AddListener(OnHeroUnlocked);
            GlobalEvents.E_OnMercLevelUp.AddListener(OnMercLeveledUp);
        }


        public List<Vector3> UnitPositions() => mercs.Where(obj => obj.Object != null).Select(obj => obj.Object.transform.position).ToList();


        void InstantiateMerc(MercID merc) => InstantiateMerc(GameData.Get().Mercs.Get(merc));
        void InstantiateMerc(MercData merc)
        {
            GameObject o = Instantiate(merc.Prefab, new Vector3(Camera.main.MinBounds().x, 6.0f), Quaternion.identity);

            o.GetComponent<MercController>().Setup(merc.Id);

            mercs.Add(new SpawnedUnit() { ID = merc.Id, Object = o });
        }


        void RotateMerc(MercID merc) => RotateMerc(GameData.Get().Mercs.Get(merc));
        void RotateMerc(MercData merc)
        {
            SpawnedUnit weakest = GetWeakestUnit();

            Destroy(weakest.Object);

            mercs.Remove(weakest);

            GameObject o = Instantiate(merc.Prefab, weakest.Object.transform.position, Quaternion.identity);

            o.GetComponent<MercController>().Setup(merc.Id);

            mercs.Add(new SpawnedUnit() { ID = merc.Id, Object = o });
        }


        SpawnedUnit GetWeakestUnit() => mercs.OrderBy(ele => StatsCache.TotalMercDamage(ele.ID)).First();


        bool MercInFormation(MercID mercId) => mercs.Where(ele => ele.ID == mercId).Count() == 1;
        bool MercIsStrongerThanWeakest(MercID mercid) => StatsCache.TotalMercDamage(mercid) > StatsCache.TotalMercDamage(GetWeakestUnit().ID);


        void OnHeroUnlocked(MercID merc)
        {
            if (mercs.Count < 5)
                InstantiateMerc(merc);

            else
                RotateMerc(merc);        
        }


        void OnMercLeveledUp(MercID merc)
        {
            if (!MercInFormation(merc) && MercIsStrongerThanWeakest(merc))
            {
                RotateMerc(merc);
            }
        }
    }
}