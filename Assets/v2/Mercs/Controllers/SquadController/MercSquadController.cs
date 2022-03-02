using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnitID = GM.Common.Enums.UnitID;


namespace GM.Mercs
{
    public class MercSquadController : Core.GMMonoBehaviour
    {
        List<SquadMerc> Mercs = new List<SquadMerc>();

        // Current positions of the mercs in the squad
        public List<Vector3> MercPositions => Mercs.Select(x => x.Position).ToList();

        // = Events = //
        public UnityEvent<SquadMerc> OnUnitAddedToSquad { get; set; } = new UnityEvent<SquadMerc>();

        void Awake()
        {
            foreach (var squadMerc in GMData.Mercs.MercsInSquad)
            {
                AddMercToSquad(squadMerc);
            }
        }
        
        public void AddMercToSquad(UnitID mercId)
        {
            Vector2 pos = new Vector2(Camera.main.MinBounds().x - 1.0f, Common.Constants.CENTER_BATTLE_Y);

            StaticMercData data = App.GMData.Mercs.GetGameMerc(mercId);

            GameObject o = Instantiate(data.Prefab, pos, Quaternion.identity);

            var merc = new SquadMerc(o);

            Mercs.Add(merc);

            OnUnitAddedToSquad.Invoke(merc);
        }

        public void RemoveMercFromSquad(UnitID mercId)
        {
            SquadMerc merc = Mercs.First(x => x.Id == mercId);

            Mercs.Remove(merc);

            Destroy(merc.GameObject);
        }
    }
}
