using GM.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GM.Mercs.UI
{
    struct MercDamageValue
    {
        public UnitID MercId;
        public DateTime Time;
        public BigDouble Damage;

        public MercDamageValue(UnitID mercId, BigDouble dmg)
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

        void Awake()
        {
            MercSquadController squad = this.GetComponentInScene<MercSquadController>();

            squad.OnUnitAddedToSquad.AddListener(merc =>
            {
                var controller = merc.GetComponent<GM.Mercs.Controllers.MercController>();

                controller.OnDamageDealt.AddListener(dmg =>
                {
                    damageValues.Add(new MercDamageValue(controller.Id, dmg));
                });
            });

            InvokeRepeating("UpdateSummaryPopup", 0, 3);
        }

        void UpdateSummaryPopup()
        {
            damageValues = damageValues.Where(x => (DateTime.UtcNow - x.Time).TotalSeconds < 60).ToList();

            if (SummaryPopup != null)
            {
                // Group up the damage dealt per merc
                var dmg = damageValues.GroupBy(m => m.MercId).Select(x => new KeyValuePair<UnitID, BigDouble>(x.Key, x.Select(x => x.Damage).Sum())).OrderByDescending(x => x.Value);

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
