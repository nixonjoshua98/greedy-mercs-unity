using GM.Common.Enums;
using GM.Mercs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GM.Mercs.UI
{
    class MercDamageValue
    {
        public MercID MercId;
        public BigDouble Damage;

        public MercDamageValue(MercID mercId, BigDouble dmg)
        {
            MercId = mercId;
            Damage = dmg;
        }
    }

    public class MercBattleSummaryController : Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] GameObject PopupObject;

        SortedList<DateTime, MercDamageValue> MercDamageValues = new();

        private void Awake()
        {
            MercSquadController squad = this.GetComponentInScene<MercSquadController>();

            squad.E_UnitSpawned.AddListener(controller =>
            {
                controller.E_OnDamageDealt.AddListener((_, dmg) => OnDamageDealt(controller.ID, dmg));
            });
        }

        public IEnumerable<KeyValuePair<MercID, BigDouble>> GetDamageValues(TimeSpan ts)
        {
            return MercDamageValues
                .Where(x => (DateTime.UtcNow - x.Key) < ts)
                .GroupBy(x => x.Value.MercId)
                .Select(x => new KeyValuePair<MercID, BigDouble>(x.Key, x.Select(x => x.Value.Damage).Sum()))
                .OrderByDescending(x => x.Value);
        }

        public TimeSpan GetActualTimeSpan()
        {
            return MercDamageValues.Count < 2 ? TimeSpan.FromSeconds(1) : (MercDamageValues.Last().Key - MercDamageValues.First().Key);
        }

        /* Callbacks */

        public void ShowSummary()
        {
            this.InstantiateUI<MercSummaryPanel>(PopupObject).Initialize(this);
        }

        void OnDamageDealt(MercID mercId, BigDouble dmg)
        {
            MercDamageValues.Add(DateTime.UtcNow, new MercDamageValue(mercId, dmg));

            MercDamageValues.RemoveAll(dt => (DateTime.UtcNow - dt).TotalMinutes > 30);
        }
    }
}
