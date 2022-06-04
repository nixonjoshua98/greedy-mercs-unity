using GM.Common.Enums;
using GM.Mercs.Controllers;
using GM.Units;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace GM.Mercs
{
    public class MercSquadController : Core.GMMonoBehaviour
    {
        [Header("References")]
        [SerializeField] EnemyUnitCollection EnemyUnits;

        private readonly List<AbstractMercController> _units = new();

        /* Events */
        public UnityEvent E_MercAddedToSquad = new UnityEvent();
        public UnityEvent<AbstractMercController> E_UnitSpawned = new();

        public int Count => _units.Count;
        public AbstractMercController First() => _units[0];

        public List<AbstractMercController> GetUnitsInFront(int idx)
        {
            List<AbstractMercController> ls = new();

            idx--; // Ignore the current unit

            while (idx > 0)
                ls.Append(_units[--idx]);

            return ls;
        }


        public AbstractMercController Get(int idx) => _units[idx];
        public int GetIndex(AbstractMercController unit) => _units.FindIndex((u) => u == unit);
        bool UnitExistsInQueue(MercID unit) => _units.Find(x => x.ID == unit) is not null;

        private void Awake()
        {
            SubscribeToEvents();

            App.Mercs.MercsInSquad.ForEach(merc =>
            {
                E_MercAddedToSquad.Invoke();
            });
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
            AbstractMercController unit = InstantiateMerc(unitId);

            unit.Init(payload, this, EnemyUnits);

            unit.E_OnZeroEnergy.AddListener(() => _units.Remove(unit)); // Remove the unit from the queue once its energy has depleted

            _units.Add(unit);

            E_UnitSpawned.Invoke(unit);
        }

        private AbstractMercController InstantiateMerc(MercID unitId)
        {
            var camBounds = Camera.main.Bounds();

            Vector2 pos = new(
                camBounds.min.x - 3.5f - (_units.Count), 
                Common.Constants.CENTER_BATTLE_Y
            );

            var data = App.Mercs.GetMerc(unitId);

            GameObject o = Instantiate(data.Prefab, pos, Quaternion.identity);

            return o.GetComponent<AbstractMercController>();
        }

        public void AddUnitToSquad(MercID mercId)
        {
            App.Mercs.SquadMercs.Add(mercId);

            E_MercAddedToSquad.Invoke();
        }

        public void RemoveUnitFromSquad(MercID mercId)
        {
            App.Mercs.SquadMercs.Remove(mercId);
        }

        // Event Listeners //

        private void GMApplication_OnMercUnlocked(MercID mercId)
        {
            if (!App.Mercs.IsSquadFull)
            {
                AddUnitToSquad(mercId);
            }
        }
    }
}