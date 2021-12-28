using GM.Mercs.Controllers;
using GM.Mercs.Data;
using GM.Mercs.Models;
using GM.Targets;
using GM.Units.Formations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MercID = GM.Common.Enums.MercID;

namespace GM
{
    public class SquadMerc
    {
        public MercController Controller { get; private set; }
        public GameObject GameObject { get; private set; }

        // Properties
        public Vector3 Position => GameObject.transform.position;

        public SquadMerc(GameObject obj)
        {
            GameObject = obj;
            Controller = obj.GetComponent<MercController>();
        }
    }


    public class MercSquadController : Core.GMMonoBehaviour
    {
        public static MercSquadController Instance = null;
        
        [SerializeField]
        UnitFormation MercFormation;

        public List<SquadMerc> Squad { get; set; } = new List<SquadMerc>();

        // Properties
        public List<Vector3> MercPositions => Squad.Select(x => x.Position).ToList();

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            App.Events.OnMercAddedToSquad.AddListener((merc) =>
            {
                Vector2 pos = new Vector2(Camera.main.MinBounds().x, Common.Constants.CENTER_BATTLE_Y);

                AddMercToSquad(merc, pos);
            });

            App.Events.OnMercRemovedFromSquad.AddListener(RemoveMercFromSquad);
        }


        public List<Vector2> MoveMercsToStageBossFormation(UnitTarget boss, Action onMercsInFormation)
        {
            return MoveMercsToFormation((merc) => { merc.Controller.LookAt(boss.Position); }, onMercsInFormation);
        }


        /// <summary> Take control of each merc and move them into formation </summary>
        public List<Vector2> MoveMercsToFormation(Action<SquadMerc> onMercReachedPosition, Action onAllMercsReachedPosition)
        {
            int mercsMoved = 0;

            Vector2 centerPosition = new Vector3(MercPositions.Average(pos => pos.x) + 1.5f, Common.Constants.CENTER_BATTLE_Y);

            List<Vector2> positions = MercFormation.Positions(centerPosition);

            int numFormationPositions = Math.Min(positions.Count, Squad.Count);

            for (int i = 0; i < numFormationPositions; ++i)
            {
                SquadMerc merc = Squad[i];

                merc.Controller.Pause();

                Vector3 position = positions[i].ToVector3();

                MoveMercToFormationPosition(merc, position, (merc) =>
                {
                    mercsMoved++;

                    onMercReachedPosition.Invoke(merc);

                    if (mercsMoved == Squad.Count)
                    {
                        onAllMercsReachedPosition();
                    }
                });
            }

            return positions.Where((pos, idx) => idx < numFormationPositions).ToList();
        }

        void MoveMercToFormationPosition(SquadMerc merc, Vector3 position, Action<SquadMerc> mercCallback)
        {
            merc.Controller.MoveTowards(position, () =>
            {
                mercCallback.Invoke(merc);
            });
        }

        void AddMercToSquad(MercID mercId, Vector2 pos)
        {
            MercGameDataModel data = App.Data.Mercs.GetGameMerc(mercId);

            GameObject o = Instantiate(data.Prefab, pos, Quaternion.identity);

            Squad.Add(new SquadMerc(o));
        }

        void RemoveMercFromSquad(MercID mercId)
        {
            MercData data = App.Data.Mercs.GetMerc(mercId);

            SquadMerc squadMerc = Squad.FirstOrDefault(merc => merc.Controller.Id == mercId);

            if (!data.InSquad && squadMerc != null)
            {
                Squad.Remove(squadMerc);

                Destroy(squadMerc.GameObject);
            }
        }
    }
}
