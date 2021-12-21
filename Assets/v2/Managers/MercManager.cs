using GM.Mercs.Controllers;
using GM.Targets;
using GM.Units.Formations;
using System;
using System.Collections.Generic;
using System.Linq;
using GM.Units;
using UnityEngine;
using UnityEngine.Events;
using DamageClickController = GM.Controllers.DamageClickController;
using MercID = GM.Common.Enums.MercID;


namespace GM
{
    public class MercManager : Core.GMMonoBehaviour
    {
        public static MercManager Instance = null;

        [SerializeField]
        UnitFormation MercFormation;

        public TargetList<MercUnitTarget> Mercs { get; private set; } = new TargetList<MercUnitTarget>();

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            App.Data.Mercs.E_MercUnlocked.AddListener((merc) =>
            {
                Vector2 pos = new Vector2(Camera.main.MinBounds().x, Common.Constants.CENTER_BATTLE_Y);

                AddMercToSquad(merc, pos);
            });
        }

        public void ResumeAndSetPriorityTarget(Target target)
        {
            Mercs.ForEach(merc =>
            {
                merc.Controller.Resume();
                merc.Controller.SetPriorityTarget(target);
            });
        }

        public List<Vector2> MoveMercsToStageBossFormation(UnitTarget boss, Action onMercsInFormation)
        {
            return MoveMercsToFormation((merc) => { merc.Controller.LookAt(boss.Position); }, onMercsInFormation);
        }

        void AddMercToSquad(MercID merc, Vector2 pos)
        {
            Mercs.Models.MercGameDataModel data = App.Data.Mercs.GetGameMerc(merc);

            GameObject o = Instantiate(data.Prefab, pos, Quaternion.identity);

            Mercs.Add(new MercUnitTarget(o));
        }


        /// <summary> Take control of each merc and move them into formation </summary>
        public List<Vector2> MoveMercsToFormation(Action<MercUnitTarget> onMercReachedPosition, Action onAllMercsReachedPosition)
        {
            int mercsMoved = 0;

            Vector2 centerPosition = new Vector3(Mercs.Average(x => x.Position.x) + 1.5f, Common.Constants.CENTER_BATTLE_Y);

            List<Vector2> positions = MercFormation.Positions(centerPosition);

            int numFormationPositions = Math.Min(positions.Count, Mercs.Count);

            for (int i = 0; i < numFormationPositions; ++i)
            {
                MercUnitTarget merc = Mercs[i];

                merc.Controller.Pause();

                Vector3 position = positions[i].ToVector3();

                MoveMercToFormationPosition(merc, position, (merc) =>
                {
                    mercsMoved++;

                    onMercReachedPosition.Invoke(merc);

                    if (mercsMoved == Mercs.Count)
                    {
                        onAllMercsReachedPosition();
                    }
                });
            }

            return positions.Where((pos, idx) => idx < numFormationPositions).ToList();
        }

        void MoveMercToFormationPosition(MercUnitTarget merc, Vector3 position, Action<MercUnitTarget> mercCallback)
        {
            merc.Controller.MoveTowards(position, () =>
            {
                mercCallback.Invoke(merc);
            });
        }
    }
}
