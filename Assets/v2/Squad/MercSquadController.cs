using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MercID = GM.Common.Enums.MercID;

namespace GM.Mercs
{
    public interface ISquadController
    {
        UnityEvent E_MercAddedToSquad { get; }

        bool TryGetUnit(out MercBaseClass unit);
        void RemoveFromQueue(MercBaseClass unit);
        void RemoveFromSquad(MercID mercId);
        void AddToSquad(MercID mercId);
        int GetQueuePosition(MercBaseClass unit);
        MercBaseClass GetUnitAtQueuePosition(int idx);
    }


    public class MercSquadController : Core.GMMonoBehaviour, ISquadController
    {
        private readonly List<MercBaseClass> UnitQueue = new List<MercBaseClass>();
        private readonly List<MercID> UnitIDs = new List<MercID>();

        public UnityEvent E_MercAddedToSquad { get; private set; } = new UnityEvent();
        public UnityEvent<MercBaseClass> E_UnitSpawned { get; set; } = new UnityEvent<MercBaseClass>();

        private void Awake()
        {
            SubscribeToEvents();

            App.Mercs.MercsInSquad.ForEach(merc => AddToSquad(merc));
        }

        private void SubscribeToEvents()
        {
            App.Mercs.E_OnMercUnlocked.AddListener(GMApplication_OnMercUnlocked);
        }

        private void FixedUpdate()
        {
            UpdateMercsEnergy();
        }

        private void UpdateMercsEnergy()
        {
            float ts = Time.fixedDeltaTime;

            foreach (MercID unit in App.Mercs.MercsInSquad)
            {
                var merc = App.Mercs.GetMerc(unit);

                float energyGained = merc.EnergyGainedPerSecond * ts;

                // Reduce the energy gained if we are 'over-charging' for extra damage
                if (merc.CurrentSpawnEnergy >= merc.SpawnEnergyRequired)
                    energyGained /= 4;

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

        public void RemoveFromQueue(MercBaseClass unit)
        {
            UnitQueue.Remove(unit);
            UnitIDs.Remove(unit.Id);
        }

        private void AddMercToQueue(MercID unitId, MercSetupPayload payload)
        {
            MercBaseClass unit = InstantiateMerc(unitId, payload);

            UnitQueue.Add(unit);
            UnitIDs.Add(unitId);

            E_UnitSpawned.Invoke(unit);
        }

        private bool UnitExistsInQueue(MercID unit)
        {
            return UnitIDs.Contains(unit);
        }

        public bool TryGetUnit(out MercBaseClass unit)
        {
            unit = UnitQueue.Count == 0 ? null : GetUnitAtQueuePosition(0);
            return unit != null;
        }

        public MercBaseClass GetUnitAtQueuePosition(int idx)
        {
            return UnitQueue[idx];
        }

        public int GetQueuePosition(MercBaseClass unit)
        {
            return UnitQueue.FindIndex((u) => u == unit);
        }

        private MercBaseClass InstantiateMerc(MercID unitId)
        {
            Vector2 pos = new Vector2(Camera.main.MinBounds().x - 3.5f, Common.Constants.CENTER_BATTLE_Y);

            StaticMercData data = App.Mercs.GetGameMerc(unitId);

            GameObject o = Instantiate(data.Prefab, pos, Quaternion.identity);

            MercBaseClass mercBase = o.GetComponent<MercBaseClass>();

            return mercBase;
        }

        private MercBaseClass InstantiateMerc(MercID unitId, MercSetupPayload payload)
        {
            MercBaseClass mercBase = InstantiateMerc(unitId);

            mercBase.Init(payload);

            return mercBase;
        }

        public void AddToSquad(MercID mercId)
        {
            App.Mercs.SquadMercs.Add(mercId);

            E_MercAddedToSquad.Invoke();
        }

        public void RemoveFromSquad(MercID mercId)
        {
            App.Mercs.SquadMercs.Remove(mercId);
        }

        // Event Listeners //

        private void GMApplication_OnMercUnlocked(MercID mercId)
        {
            if (!App.Mercs.IsSquadFull)
            {
                AddToSquad(mercId);
            }
        }
    }
}
