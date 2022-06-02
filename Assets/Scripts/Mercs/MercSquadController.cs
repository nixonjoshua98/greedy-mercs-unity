using GM.Mercs.Controllers;
using GM.Units;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MercID = GM.Common.Enums.MercID;

namespace GM.Mercs
{
    public class MercSquadController : Core.GMMonoBehaviour
    {
        [Header("References")]
        [SerializeField] UnitCollection EnemyUnits;

        private readonly List<MercBase> UnitQueue = new List<MercBase>();
        private readonly List<MercID> UnitIDs = new List<MercID>();

        public UnityEvent E_MercAddedToSquad { get; private set; } = new UnityEvent();
        public UnityEvent<MercBase> E_UnitSpawned { get; set; } = new UnityEvent<MercBase>();


        public void Remove(MercBase unit)
        {
            UnitQueue.Remove(unit);
            UnitIDs.Remove(unit.Id);
        }

        public bool TryGetUnit(out MercBase unit)
        {
            unit = UnitQueue.Count == 0 ? null : UnitQueue[0];

            return unit != null;
        }

        public MercBase Get(int idx) => UnitQueue[idx];
        public int GetIndex(MercBase unit) => UnitQueue.FindIndex((u) => u == unit);

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

        private void AddMercToQueue(MercID unitId, MercSetupPayload payload)
        {
            MercBase unit = InstantiateMerc(unitId, payload);

            UnitQueue.Add(unit);
            UnitIDs.Add(unitId);

            E_UnitSpawned.Invoke(unit);
        }

        bool UnitExistsInQueue(MercID unit)
        {
            return UnitIDs.Contains(unit);
        }

        private MercBase InstantiateMerc(MercID unitId)
        {
            Vector2 pos = new Vector2(Camera.main.Bounds().min.x - 3.5f, Common.Constants.CENTER_BATTLE_Y);

            var data = App.Mercs.GetMerc(unitId);

            GameObject o = Instantiate(data.Prefab, pos, Quaternion.identity);

            MercBase mercBase = o.GetComponent<MercBase>();

            return mercBase;
        }

        private MercBase InstantiateMerc(MercID unitId, MercSetupPayload payload)
        {
            MercController merc = InstantiateMerc(unitId) as MercController;

            merc.Init(payload, EnemyUnits);

            return merc;
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
