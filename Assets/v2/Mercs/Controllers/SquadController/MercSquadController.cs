using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnitID = GM.Common.Enums.UnitID;
using GM.Units;

namespace GM.Mercs
{
    public interface ISquadController
    {
        void AddMercToSquad(UnitID mercId);
        void RemoveMercFromSquad(UnitID mercId);
        int GetQueuePosition(UnitID unit);
        UnitBaseClass GetUnitAtQueuePosition(int idx);
    }


    public class MercSquadController : Core.GMMonoBehaviour, ISquadController
    {
        Dictionary<UnitID, UnitBaseClass> Units { get; set; } = new Dictionary<UnitID, UnitBaseClass>();


        // Current positions of the mercs in the squad
        public List<Vector3> MercPositions => Units.Values.Select(x => x.gameObject.transform.position).ToList();

        // = Events = //
        public UnityEvent<UnitBaseClass> OnUnitAddedToSquad { get; set; } = new UnityEvent<UnitBaseClass>();

        void Awake()
        {
            foreach (var squadMerc in GMData.Mercs.MercsInSquad)
            {
                AddMercToSquad(squadMerc);
            }
        }

        public UnitBaseClass GetUnitAtQueuePosition(int idx) => Units[Units.Keys.ToList()[idx]];
        public int GetQueuePosition(UnitID unit) => Units.Keys.FindIndexWhere(x => x == unit);


        public void AddMercToSquad(UnitID mercId)
        {
            Vector2 pos = new Vector2(Camera.main.MinBounds().x - 1.0f, Common.Constants.CENTER_BATTLE_Y);

            StaticMercData data = App.GMData.Mercs.GetGameMerc(mercId);

            GameObject o = Instantiate(data.Prefab, pos, Quaternion.identity);

            UnitBaseClass unit = o.GetComponent<UnitBaseClass>();

            Units[mercId] = unit;

            App.PersistantLocalFile.SquadMercIDs.Add(mercId);

            OnUnitAddedToSquad.Invoke(unit);
        }

        public void RemoveMercFromSquad(UnitID mercId)
        {
            App.PersistantLocalFile.SquadMercIDs.Remove(mercId);

            UnitBaseClass unit = Units[mercId];

            Units.Remove(mercId);

            Destroy(unit.gameObject);
        }
    }
}
