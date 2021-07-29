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
    using GM.Units.Formations;

    struct SpawnedUnit
    {
        public MercID ID;
        public GameObject Object;
        public MercController Controller;
    }



    public class SquadManager : MonoBehaviour
    {
        public static SquadManager Instance = null;

        List<SpawnedUnit> mercs;

        [SerializeField] UnitFormation formation;

        void Awake()
        {
            Instance = this;

            mercs = new List<SpawnedUnit>();
        }


        void Start()
        {
            SubscribeToEvents();
        }


        void SubscribeToEvents()
        {
            GlobalEvents.E_OnMercUnlocked.AddListener(OnHeroUnlocked);
            GlobalEvents.E_OnMercLevelUp.AddListener(OnMercLeveledUp);

            GameManager.Get.E_OnBossSpawn.AddListener(OnBossSpawn);
        }


        // = = = Public = = = //
        public List<Vector3> UnitPositions => mercs.Where(obj => obj.Object != null).Select(obj => obj.Object.transform.position).ToList();
        // = = = ^


        void InstantiateMerc(MercID merc) => InstantiateMerc(GameData.Get().Mercs.Get(merc));
        void InstantiateMerc(MercData merc)
        {
            GameObject o = Instantiate(merc.Prefab, new Vector3(Camera.main.MinBounds().x, 6.0f), Quaternion.identity);

            MercController controller = o.GetComponent<MercController>();

            controller.Setup(merc.Id);

            mercs.Add(new SpawnedUnit()
            {
                ID = merc.Id,
                Object = o,
                Controller = controller
            });
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


        // = = = Event Listeners = = = //
        void OnBossSpawn(GameObject boss)
        {
            Vector3 cameraPosition = Camera.main.MinBounds();

            float offsetX = Mathf.Abs(formation.MinBounds().x) + 1.0f;

            for (int i = 0; i < mercs.Count; ++i)
            {
                SpawnedUnit unit = mercs[i];

                Vector2 relPos = formation.GetPosition(i);

                Vector2 targetPosition = new Vector2(offsetX + cameraPosition.x + relPos.x, relPos.y + Constants.CENTER_BATTLE_Y);

                unit.Controller.PriorityMove(targetPosition, (controller) =>
                {
                    controller.Attack.Enable();
                });
            }
        }


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