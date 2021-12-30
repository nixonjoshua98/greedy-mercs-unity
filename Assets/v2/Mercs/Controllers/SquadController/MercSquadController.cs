using GM.Mercs.Models;
using GM.Units.Formations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using MercID = GM.Common.Enums.MercID;


namespace GM.Mercs
{
    public class MercSquadController : Core.GMMonoBehaviour
    {      
        [SerializeField] private UnitFormation MercFormation;

        // Array which can contain nulls to preserve indexing
        SquadMerc[] FormationSpots { get; set; } = new SquadMerc[Common.Constants.MAX_SQUAD_SIZE];

        // Return only non-null elements
        public List<SquadMerc> Mercs => FormationSpots.Where(x => x != null).ToList();

        // Current positions of the mercs in the squad
        public List<Vector3> MercPositions => Mercs.Select(x => x.Position).ToList();

        // Formation positions used i nthe current (or previous) formation
        List<Vector2> CurrentFormationPositions;

        // = States = //
        public bool IsMovingToFormation { get; private set; }

        // = Events = //
        public UnityEvent<SquadMerc> OnUnitAddedToSquad { get; set; } = new UnityEvent<SquadMerc>();

        /// <summary> Take control of each merc and move them into formation </summary>
        public void MoveMercsToFormation(Action<SquadMerc> mercCallback, Action callback)
        {
            IEnumerator coro = Enumerators.InvokeAfter(MoveSquadIntoFormation(mercCallback), callback);

            StartCoroutine(coro);
        }

        IEnumerator MoveSquadIntoFormation(Action<SquadMerc> onMercReachedPosition)
        {
            Vector2 center = new Vector3(MercPositions.Average(pos => pos.x) + 1.5f, Common.Constants.CENTER_BATTLE_Y);

            // Set the formation positions so we can use it elsewhere
            CurrentFormationPositions = MercFormation.Positions(center);

            // Set some state variables
            bool isAnyMercMoving = true;
            IsMovingToFormation = true;

            // Prepare a dictionary for distances
            var distanceFromSpots = new Dictionary<int, float>();

            while (isAnyMercMoving)
            {
                yield return new WaitForFixedUpdate();

                isAnyMercMoving = false;

                // Iterate over the spots to keep indexes
                for (int i = 0; i < FormationSpots.Length; ++i)
                {
                    SquadMerc merc = FormationSpots[i];

                    if (merc == null)
                    {
                        distanceFromSpots[i] = float.MaxValue;
                        continue;
                    }

                    merc.Controller.Pause(); // Take control of the unit

                    Vector3 position = CurrentFormationPositions[i].ToVector3();

                    // Current distance to the target
                    float distToPosition = Vector2.Distance(merc.Position, position);

                    // Distance from the target last update. Used to check if the unit has just reached the spot
                    float prevDistance = distanceFromSpots.Get(i, float.MaxValue);

                    // Unit is now in position
                    if (prevDistance > 0 && distToPosition == 0)
                    {
                        onMercReachedPosition.Invoke(merc);

                        merc.Controller.Idle();
                    }

                    // Unit needs to move towards the position
                    else if (distToPosition > 0)
                    {
                        isAnyMercMoving = true;

                        merc.Controller.MoveTowards(position);
                    }

                    distanceFromSpots[i] = distToPosition;
                }
            }

            IsMovingToFormation = false;
        }

        public bool AddMercToSquad(MercID mercId)
        {
            Vector2 pos = new Vector2(Camera.main.MinBounds().x - 1.0f, Common.Constants.CENTER_BATTLE_Y);

            MercGameDataModel data = App.Data.Mercs.GetGameMerc(mercId);

            GameObject o = Instantiate(data.Prefab, pos, Quaternion.identity);

            int squadIndex = GetAvailableFormationSpot();

            if (squadIndex >= 0)
            {
                var merc = new SquadMerc(o);

                FormationSpots[squadIndex] = merc;

                OnUnitAddedToSquad.Invoke(merc);

                return true;
            }
            else
            {
                return false;
            }
        }

        public void MoveMercToFormationPosition(MercID mercId)
        {
            int index = GetFormationIndex(mercId);

            if (index >= 0)
            {
                SquadMerc merc = FormationSpots[index];

                merc.Position = CurrentFormationPositions[index];

                merc.Controller.Idle();
            }
            else
            {
                Debug.Log("Fatal: Attempted to move idle merc to formation");
            }
        }

        public bool RemoveMercFromSquad(MercID mercId)
        {
            int index = GetFormationIndex(mercId);

            if (index >= 0)
            {
                Destroy(FormationSpots[index].GameObject);

                FormationSpots[index] = null;

                return true;
            }
            else
            {
                return false;
            }
        }

        int GetFormationIndex(MercID mercId)
        {
            return FormationSpots.FindIndexWhere(merc => merc != null && merc.Controller.Id == mercId);
        }

        int GetAvailableFormationSpot()
        {
            return Array.FindIndex(FormationSpots, (merc) => merc == null);
        }
    }
}
