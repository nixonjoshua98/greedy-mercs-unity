using GM.Mercs.Data;
using GM.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace GM.Mercs.UI
{
    public class MercSummaryPanel : GM.UI.PopupBase
    {
        [Header("Text Elements")]
        [SerializeField] TMP_Text DPSText;
        [Space]
        [SerializeField] GameObject MercSummarySlotObject;
        [SerializeField] Transform MercSlotsParent;
        [Space]
        [SerializeField] IntegerSelector Selector;

        List<MercDamageSummarySlot> SummarySlots = new();

        MercBattleSummaryController SummaryController;

        TimeSpan CurrentTimeSpan;

        void Awake()
        {
            CurrentTimeSpan = TimeSpan.FromSeconds(Selector.CurrentValue);

            Selector.E_OnChange.AddListener(secs =>
            {
                CurrentTimeSpan = TimeSpan.FromSeconds(secs);

                UpdateSummarySlots();
            });
        }

        void Start()
        {
            ShowInnerPanel();

            InvokeRepeating(nameof(UpdateSummarySlots), 0.0f, 1.0f);
        }

        public void Initialize(MercBattleSummaryController summaryController)
        {
            SummaryController = summaryController;
        }

        void UpdateSummarySlots()
        {
            var damageValues = SummaryController.GetDamageValues(CurrentTimeSpan).ToList();

            BigDouble totalDamage = BigDouble.Max(1, damageValues.Select(x => x.Value).Sum());

            for (int i = 0; i < Mathf.Max(damageValues.Count, SummarySlots.Count); ++i)
            {
                if (i >= SummarySlots.Count)
                    InstantiateMercSummarySlot();

                MercDamageSummarySlot slot = SummarySlots[i];

                slot.SetEmpty();

                if (i < damageValues.Count)
                {
                    MercID mercId = damageValues[i].Key;
                    BigDouble damageDealt = damageValues[i].Value;

                    float percent = (float)(damageDealt / totalDamage).ToDouble();

                    slot.UpdateValues(mercId, damageDealt, percent);
                }
            }

            DPSText.text = Format.Number(totalDamage / SummaryController.GetActualTimeSpan().TotalSeconds);
        }

        private void InstantiateMercSummarySlot()
        {
            GameObject go = Instantiate(MercSummarySlotObject, MercSlotsParent);

            MercDamageSummarySlot slot = go.GetComponent<MercDamageSummarySlot>();

            SummarySlots.Add(slot);
        }

        /* Callbacks */

        public void CloseModal()
        {
            Destroy(gameObject);
        }
    }
}