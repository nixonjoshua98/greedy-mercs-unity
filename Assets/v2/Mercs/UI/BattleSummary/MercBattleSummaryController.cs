using GM.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GM.Mercs.UI
{
    struct MercDamageValue
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
        [SerializeField] MercSquadController MercSquad;

        [Header("Prefabs")]
        public GameObject PopupObject;

        List<MercDamageValue> damageValues = new List<MercDamageValue>();

        MercBattleSummaryPopup SummaryPopup;

        void Awake()
        {
            MercSquad.OnUnitAddedToSquad.AddListener(merc =>
            {
                merc.Controller.OnDamageDealt.AddListener(dmg =>
                {
                    damageValues.Add(new MercDamageValue(merc.Id, dmg));
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
                var dmg = damageValues.GroupBy(m => m.MercId).Select(x => new KeyValuePair<MercID, BigDouble>(x.Key, x.Select(x => x.Damage).Sum())).OrderByDescending(x => x.Value);

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
