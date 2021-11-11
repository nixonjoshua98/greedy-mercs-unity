using GM.Targets;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MercID = GM.Common.Enums.MercID;

namespace GM
{

    using GM.Units;
    using GM.Units.Formations;

    struct SpawnedUnit
    {
        public GameObject Object { get; set; }
        public MercController Controller { get; set; }
        public GM.Mercs.Controllers.MercController NewController { get; set; }
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
            App.Data.Mercs.E_MercUnlocked.AddListener(OnHeroUnlocked);

            GameManager.Instance.E_BossSpawn.AddListener(OnBossSpawn);
        }


        // = = = Public = = = //
        public List<Vector3> UnitPositions => mercs.Where(obj => obj.Object != null).Select(obj => obj.Object.transform.position).ToList();
        // = = = ^

        void InstantiateAndSetupMerc(MercID merc, Vector2 pos)
        {
            Mercs.Models.MercGameDataModel data = App.Data.Mercs.GetGameMerc(merc);

            GameObject o = Instantiate(data.Prefab, pos, Quaternion.identity);

            MercController controller = o.GetComponent<MercController>();

            if (controller != null)
            {
                controller.Setup(merc);
            }

            mercs.Add(new SpawnedUnit()
            {
                Object = o,
                Controller = controller,
                NewController = o.GetComponent<GM.Mercs.Controllers.MercController>()
            });
        }


        // = = = Event Listeners = = = //
        void OnBossSpawn(GameObject boss)
        {
            Vector3 cameraPosition = Camera.main.MinBounds();

            float offsetX = Mathf.Abs(formation.MinBounds().x) + 1.0f;

            for (int i = 0; i < Mathf.Min(formation.NumPositions, mercs.Count); ++i)
            {
                SpawnedUnit unit = mercs[i];

                Vector2 relPos = formation.GetPosition(i);

                Vector2 targetPosition = new Vector2(offsetX + cameraPosition.x + relPos.x, relPos.y + Constants.CENTER_BATTLE_Y);

                if (unit.Controller != null)
                {
                    unit.Controller.PriorityMove(targetPosition, (controller) =>
                    {
                        controller.Attack.Enable();
                    });
                }
                else
                {
                    Target target = new Target(boss, targetPosition);

                    unit.NewController.AssignTarget(target);
                }
            }
        }


        void OnHeroUnlocked(MercID merc)
        {
            Vector2 pos = new Vector2(Camera.main.MinBounds().x, Constants.CENTER_BATTLE_Y);

            InstantiateAndSetupMerc(merc, pos);
        }
    }
}