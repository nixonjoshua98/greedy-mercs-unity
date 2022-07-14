using GM.Enums;
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

        [Header("Events")]
        [HideInInspector] public UnityEvent<AbstractMercController> E_UnitSpawned = new();

        private readonly List<AbstractMercController> _units = new();


        public AbstractMercController Get(int idx) => _units[idx];
        public int GetIndex(AbstractMercController unit) => _units.FindIndex((u) => u == unit);
        bool UnitExistsInQueue(MercID unit) => _units.Find(x => x.ID == unit) is not null;

        private void FixedUpdate()
        {
            UpdateMercsEnergy();
        }

        private void UpdateMercsEnergy()
        {
            foreach (var merc in App.Mercs.UnlockedMercs)
            {
                float ts = Time.fixedDeltaTime;

                // Reduce the timer gained if we are 'over-charging' for extra damage
                if (merc.RechargeProgress >= merc.RechargeRate)
                    ts /= 4;

                // Increment the value
                merc.RechargeProgress = Mathf.Min(merc.RechargeProgress + ts, merc.RechargeRate * 2);

                // Check if we can spawn a new unit in the queue
                if (merc.RechargePercentage >= 1.0f && !UnitExistsInQueue(merc.ID))
                {
                    // Create payload
                    MercSetupPayload payload = new(merc.RechargePercentage);

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

            Vector2 pos = new(camBounds.min.x - 3.5f - (_units.Count), Common.Constants.CENTER_BATTLE_Y);

            var data = App.Mercs.GetMerc(unitId);

            GameObject o = Instantiate(data.Prefab, pos, Quaternion.identity);

            return o.GetComponent<AbstractMercController>();
        }
    }
}