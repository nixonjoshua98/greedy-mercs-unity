using SRC.Mercs.Data;
using SRC.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace SRC.Mercs.UI
{
    public class MercSummaryPanel : SRC.UI.PopupBase
    {
        [Header("Text Elements")]
        [SerializeField] private TMP_Text DPSText;
        [Space]
        [SerializeField] private GameObject MercSummarySlotObject;
        [SerializeField] private Transform MercSlotsParent;
        [Space]
        [SerializeField] private IntegerSelector Selector;
        private readonly List<MercDamageSummarySlot> SummarySlots = new();
        private MercBattleSummaryController SummaryController;
        private TimeSpan CurrentTimeSpan;

        private void Awake()
        {
            CurrentTimeSpan = TimeSpan.FromSeconds(Selector.CurrentValue);

            Selector.E_OnChange.AddListener(secs =>
            {
                CurrentTimeSpan = TimeSpan.FromSeconds(secs);

                UpdateSummarySlots();
            });
        }

        private void Start()
        {
            ShowInnerPanel();

            InvokeRepeating(nameof(UpdateSummarySlots), 0.0f, 1.0f);
        }

        public void Initialize(MercBattleSummaryController summaryController)
        {
            SummaryController = summaryController;
        }

        private void UpdateSummarySlots()
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