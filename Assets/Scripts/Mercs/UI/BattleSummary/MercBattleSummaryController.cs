using GM.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GM.Mercs.UI
{
    internal struct MercDamageValue
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
        private List<MercDamageValue> damageValues = new List<MercDamageValue>();
        private MercBattleSummaryPopup SummaryPopup;

        private void Awake()
        {
            MercSquadController squad = this.GetComponentInScene<MercSquadController>();

            squad.E_UnitSpawned.AddListener(controller =>
            {
                controller.OnDamageDealt.AddListener(dmg =>
                {
                    damageValues.Add(new MercDamageValue(controller.ID, dmg));
                });
            });

            InvokeRepeating("UpdateSummaryPopup", 0, 3);
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
            SummaryPopup = InstantiateUI<MercBattleSummaryPopup>(PopupObject);

            UpdateSummaryPopup();
        }
    }
}
