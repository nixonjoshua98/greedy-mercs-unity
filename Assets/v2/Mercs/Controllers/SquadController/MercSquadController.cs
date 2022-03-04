using GM.Units;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnitID = GM.Common.Enums.UnitID;

namespace GM.Mercs
{
    public interface ISquadController
    {
        bool TryGetFrontUnitQueue(out UnitBaseClass unit);

        void AddMercToSquad(UnitID mercId);
        void RemoveMercFromSquad(UnitID mercId);
        int GetQueuePosition(UnitBaseClass unit);
        UnitBaseClass GetUnitAtQueuePosition(int idx);
    }


    public class MercSquadController : Core.GMMonoBehaviour, ISquadController
    {
        List<UnitBaseClass> UnitQueue = new List<UnitBaseClass>();

        public UnityEvent<UnitBaseClass> E_UnitSpawned { get; set; } = new UnityEvent<UnitBaseClass>();

        void Awake()
        {
            GMData.Mercs.MercsInSquad.ForEach(merc => AddMercToSquad(merc));
        }

        void FixedUpdate()
        {
            UpdateMercsEnergy();
        }

        void UpdateMercsEnergy()
        {
            float ts = Time.fixedUnscaledDeltaTime;

            foreach (UnitID unit in App.GMData.Mercs.MercsInSquad)
            {
                var merc = App.GMData.Mercs.GetMerc(unit);

                float energyGained = merc.EnergyGainedPerSecond * ts;

                merc.CurrentSpawnEnergy = Mathf.Min(merc.SpawnEnergyRequired, merc.CurrentSpawnEnergy + energyGained);

                if (merc.CurrentSpawnEnergy == merc.SpawnEnergyRequired)
                {
                    merc.CurrentSpawnEnergy = 0;

                    InstantiateMerc(merc.ID);
                }
            }
        }

        public bool TryGetFrontUnitQueue(out UnitBaseClass unit)
        {
            unit = UnitQueue.Count == 0 ? null : GetUnitAtQueuePosition(0);
            return unit != null;
        }

        public UnitBaseClass GetUnitAtQueuePosition(int idx) => UnitQueue[idx];
        public int GetQueuePosition(UnitBaseClass unit) => UnitQueue.FindIndex((u) => u == unit);

        void InstantiateMerc(UnitID unitId)
        {
            Vector2 pos = new Vector2(Camera.main.MinBounds().x - 1.5f, Common.Constants.CENTER_BATTLE_Y);

            StaticMercData data = App.GMData.Mercs.GetGameMerc(unitId);

            GameObject o = Instantiate(data.Prefab, pos, Quaternion.identity);

            UnitBaseClass unit = o.GetComponent<UnitBaseClass>();

            UnitQueue.Add(unit);

            E_UnitSpawned.Invoke(unit);
        }


        public void AddMercToSquad(UnitID mercId)
        {
            App.PersistantLocalFile.SquadMercIDs.Add(mercId);
        }

        public void RemoveMercFromSquad(UnitID mercId)
        {
            App.PersistantLocalFile.SquadMercIDs.Remove(mercId);
        }
    }
}
