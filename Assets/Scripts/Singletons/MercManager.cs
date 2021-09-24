using System.Collections.Generic;
using System.Linq;
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



    public class MercManager : Core.GMMonoBehaviour
    {
        public static MercManager Instance { get; set; } = null;

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

        void ReplaceWeakestMerc(MercID merc)
        {
            SpawnedUnit weakest = GetWeakestUnit();

            mercs.Remove(weakest);

            InstantiateAndSetupMerc(merc, weakest.Object.transform.position);

            Destroy(weakest.Object);
        }



        SpawnedUnit GetWeakestUnit() => mercs.OrderBy(ele => StatsCache.TotalMercDamage(ele.ID)).First();
        bool MercInFormation(MercID mercId) => mercs.Where(ele => ele.ID == mercId).Count() == 1;
        bool MercIsStrongerThanWeakest(MercID mercid) => StatsCache.TotalMercDamage(mercid) > StatsCache.TotalMercDamage(GetWeakestUnit().ID);


        void InstantiateAndSetupMerc(MercID merc, Vector2 pos)
        {
            GM.Mercs.Data.MercGameData data = App.Data.Mercs[merc].Game;

            GameObject o = Instantiate(data.Prefab, pos, Quaternion.identity);

            MercController controller = o.GetComponent<MercController>();

            controller.Setup(merc);

            mercs.Add(new SpawnedUnit()
            {
                ID = merc,
                Object = o,
                Controller = controller
            });
        }


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
            {
                Vector2 pos = new Vector2(Camera.main.MinBounds().x, Constants.CENTER_BATTLE_Y);

                InstantiateAndSetupMerc(merc, pos);
            }

            else
            {
                ReplaceWeakestMerc(merc);
            }
        }


        void OnMercLeveledUp(MercID merc)
        {
            if (!MercInFormation(merc) && MercIsStrongerThanWeakest(merc))
            {
                ReplaceWeakestMerc(merc);
            }
        }
    }
}