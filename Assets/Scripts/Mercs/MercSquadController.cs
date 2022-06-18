using GM.Common.Enums;
using GM.Mercs.Controllers;
using GM.Units;
using System.Collections.Generic;
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
        [HideInInspector] public UnityEvent E_MercAddedToSquad = new UnityEvent();
        [HideInInspector] public UnityEvent<AbstractMercController> E_UnitSpawned = new();

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
            foreach (MercID unit in App.Mercs.MercsInSquad)
            {
                float ts = Time.fixedDeltaTime;

                var merc = App.Mercs.GetMerc(unit);

                // Reduce the timer gained if we are 'over-charging' for extra damage
                if (merc.RechargeProgress >= merc.RechargeRate)
                    ts /= 4;

                // Increment the value
                merc.RechargeProgress = Mathf.Min(merc.RechargeProgress + ts, merc.RechargeRate * 2);

                // Check if we can spawn a new unit in the queue
                if (merc.RechargePercentage >= 1.0f && !UnitExistsInQueue(merc.ID))
                {
                    // Create payload
                    MercSetupPayload payload = new MercSetupPayload(merc.RechargePercentage);

                    // Reset some data
                    merc.RechargeProgress = 0;

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