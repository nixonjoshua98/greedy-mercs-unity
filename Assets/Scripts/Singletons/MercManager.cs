﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MercID = GM.Common.Enums.MercID;
using GM.Targets;

namespace GM
{

    using GM.Units;
    using GM.Units.Formations;

    struct SpawnedUnit
    {
        public GameObject Object { get; set; }
        public MercController Controller { get; set; }
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
            });
        }


        // = = = Event Listeners = = = //
        void OnBossSpawn(Target boss)
        {
            Vector3 cameraPosition = Camera.main.MinBounds();

            float offsetX = Mathf.Abs(formation.MinBounds().x) + 1.0f;

            int mercsMoved = 0;

            for (int i = 0; i < mercs.Count; ++i)
            {
                SpawnedUnit unit = mercs[i];

                Vector2 relPos = formation.GetPosition(Mathf.Min(formation.NumPositions - 1, i));

                Vector2 targetPosition = new Vector2(offsetX + cameraPosition.x + relPos.x, relPos.y + Common.Constants.CENTER_BATTLE_Y);

                if (unit.Controller != null)
                {
                    unit.Controller.PriorityMove(targetPosition, (controller) =>
                    {
                        mercsMoved++;

                        if (mercsMoved == mercs.Count)
                        {
                            foreach (SpawnedUnit unit in mercs)
                            {
                                unit.Controller.Attack.Enable();
                            }
                        }
                    });
                }
            }
        }


        void OnHeroUnlocked(MercID merc)
        {
            Vector2 pos = new Vector2(Camera.main.MinBounds().x, Common.Constants.CENTER_BATTLE_Y);

            InstantiateAndSetupMerc(merc, pos);
        }
    }
}