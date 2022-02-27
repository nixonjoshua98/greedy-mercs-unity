using GM.Mercs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnitID = GM.Common.Enums.UnitID;


namespace GM.Mercs
{
    public class MercSquadController : Core.GMMonoBehaviour
    {      
        // Array which can contain nulls to preserve indexing
        SquadMerc[] FormationSpots { get; set; } = new SquadMerc[Common.Constants.MAX_SQUAD_SIZE];

        // Return only non-null elements
        public List<SquadMerc> Mercs => FormationSpots.Where(x => x != null).ToList();

        // Current positions of the mercs in the squad
        public List<Vector3> MercPositions => Mercs.Select(x => x.Position).ToList();

        // = Events = //
        public UnityEvent<SquadMerc> OnUnitAddedToSquad { get; set; } = new UnityEvent<SquadMerc>();
        
        public bool AddMercToSquad(UnitID mercId)
        {
            Vector2 pos = new Vector2(Camera.main.MinBounds().x - 1.0f, Common.Constants.CENTER_BATTLE_Y);

            MercGameDataModel data = App.Data.Mercs.GetGameMerc(mercId);

            int squadIndex = GetAvailableFormationSpot();

            if (squadIndex >= 0)
            {
                GameObject o = Instantiate(data.Prefab, pos, Quaternion.identity);

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

        public bool RemoveMercFromSquad(UnitID mercId)
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

        int GetFormationIndex(UnitID mercId)
        {
            return FormationSpots.FindIndexWhere(merc => merc != null && merc.Controller.Id == mercId);
        }

        int GetAvailableFormationSpot()
        {
            return Array.FindIndex(FormationSpots, (merc) => merc == null);
        }
    }
}
