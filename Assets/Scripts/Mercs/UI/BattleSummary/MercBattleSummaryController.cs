using GM.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GM.Mercs.UI
{
    internal class MercDamageValue
    {
        public MercID MercId;
        public DateTime Time;
        public BigDouble Damage;

        public MercDamageValue(MercID mercId, BigDouble dmg)
        {
            Time = DateTime.UtcNow;
            MercId = mercId;
            Damage = dmg;
        }
    }

    public class MercBattleSummaryController : Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject PopupObject;

        List<MercDamageValue> damageValues = new List<MercDamageValue>();
        MercBattleSummaryPopup SummaryPopup;

        private void Awake()
        {
            MercSquadController squad = this.GetComponentInScene<MercSquadController>();

            squad.E_UnitSpawned.AddListener(controller =>
            {
                controller.E_OnEnemyDefeated.AddListener(() => OnEnemyDefeated(controller.ID));
                controller.E_OnDamageDealt.AddListener(dmg => OnDamageDealt(controller.ID, dmg));
            });

            InvokeRepeating(nameof(UpdateSummaryPopup), 0, 3);
        }

        private void UpdateSummaryPopup()
        {
            damageValues = damageValues.Where(x => (DateTime.UtcNow - x.Time).TotalSeconds < 60).ToList();

            if (SummaryPopup != null)
            {
                var dmg = damageValues
                    .GroupBy(m => m.MercId)
                    .Select(x =>
                    {
                        return new KeyValuePair<MercID, BigDouble>(x.Key, x.Select(x => x.Damage).Sum());
                    })
                    .OrderByDescending(x => x.Value);

                SummaryPopup.UpdateDamageNumbers(dmg.ToList());
            }
        }

        public void ShowSummary()
        {
            SummaryPopup = this.InstantiateUI<MercBattleSummaryPopup>(PopupObject);

            UpdateSummaryPopup();
        }

        /* Event Listeners */

        void OnDamageDealt(MercID mercId, BigDouble dmg)
        {
            damageValues.Add(new MercDamageValue(mercId, dmg));
        }

        void OnEnemyDefeated(MercID mercId)
        {
            if (App.Mercs.TryGetMercState(mercId, out var state))
            {
                state.EnemiesDefeatedSincePrestige++;
            }
        }
    }
}
