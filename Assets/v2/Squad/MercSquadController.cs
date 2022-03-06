using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnitID = GM.Common.Enums.UnitID;

namespace GM.Mercs
{
    public interface ISquadController
    {
        bool TryGetFrontUnitQueue(out MercBaseClass unit);
        void RemoveMercFromQueue(MercBaseClass unit);
        void AddMercToSquad(UnitID mercId);
        void RemoveMercFromSquad(UnitID mercId);
        int GetQueuePosition(MercBaseClass unit);
        MercBaseClass GetUnitAtQueuePosition(int idx);
    }


    public class MercSquadController : Core.GMMonoBehaviour, ISquadController
    {
        List<MercBaseClass> UnitQueue = new List<MercBaseClass>();
        List<UnitID> UnitIDs = new List<UnitID>();

        public UnityEvent<MercBaseClass> E_UnitSpawned { get; set; } = new UnityEvent<MercBaseClass>();

        void Awake()
        {
            App.GMData.Mercs.MercsInSquad.ForEach(merc => AddMercToSquad(merc));
        }

        void FixedUpdate()
        {
            UpdateMercsEnergy();
        }

        void UpdateMercsEnergy()
        {
            float ts = Time.fixedDeltaTime;

            foreach (UnitID unit in App.GMData.Mercs.MercsInSquad)
            {
                var merc = App.GMData.Mercs.GetMerc(unit);

                float energyGained = merc.EnergyGainedPerSecond * ts;

                // Reduce the energy gained if we are 'over-charging' for extra damage
                if (merc.CurrentSpawnEnergy >= merc.SpawnEnergyRequired)
                    energyGained /= ((merc.CurrentSpawnEnergy / merc.SpawnEnergyRequired) * 2);

                // Increment the energy value
                merc.CurrentSpawnEnergy = Mathf.Min(merc.CurrentSpawnEnergy + energyGained, merc.SpawnEnergyRequired * 2);

                // Check if we can spawn a new unit in the queue
                if (merc.CurrentSpawnEnergy >= merc.SpawnEnergyRequired && !UnitExistsInQueue(merc.ID))
                {
                    // Create payload
                    MercSetupPayload payload = new MercSetupPayload(merc.CurrentSpawnEnergyPercentage);

                    // Reset some data
                    merc.CurrentSpawnEnergy = 0;

                    // Add merc to queue
                    AddMercToQueue(merc.ID, payload);
                }
            }
        }

        public void RemoveMercFromQueue(MercBaseClass unit)
        {
            UnitQueue.Remove(unit);
            UnitIDs.Remove(unit.Id);
        }

        void AddMercToQueue(UnitID unitId, MercSetupPayload payload)
        {
            MercBaseClass unit = InstantiateMerc(unitId, payload);

            UnitQueue.Add(unit);
            UnitIDs.Add(unitId);

            E_UnitSpawned.Invoke(unit);
        }

        bool UnitExistsInQueue(UnitID unit) => UnitIDs.Contains(unit);

        public bool TryGetFrontUnitQueue(out MercBaseClass unit)
        {
            unit = UnitQueue.Count == 0 ? null : GetUnitAtQueuePosition(0);
            return unit != null;
        }

        public MercBaseClass GetUnitAtQueuePosition(int idx) => UnitQueue[idx];

        public int GetQueuePosition(MercBaseClass unit) => UnitQueue.FindIndex((u) => u == unit);

        MercBaseClass InstantiateMerc(UnitID unitId)
        {
            Vector2 pos = new Vector2(Camera.main.MinBounds().x - 3.5f, Common.Constants.CENTER_BATTLE_Y);

            StaticMercData data = App.GMData.Mercs.GetGameMerc(unitId);

            GameObject o = Instantiate(data.Prefab, pos, Quaternion.identity);

            MercBaseClass mercBase = o.GetComponent<MercBaseClass>();

            return mercBase;
        }

        MercBaseClass InstantiateMerc(UnitID unitId, MercSetupPayload payload)
        {
            MercBaseClass mercBase = InstantiateMerc(unitId);

            mercBase.Init(payload);

            return mercBase;
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
