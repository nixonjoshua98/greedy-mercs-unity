using GM.Common.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GM.Mercs.UI
{
    public class MercBattleSummaryPopup : MonoBehaviour
    {
        [SerializeField] private GameObject MercSummarySlotObject;
        [SerializeField] private Transform MercSlotsParent;
        private readonly List<MercDamageSummarySlot> MercSlots = new List<MercDamageSummarySlot>();

        public void UpdateDamageNumbers(List<KeyValuePair<MercID, BigDouble>> dmg)
        {
            MercSlots.ForEach(slot => slot.SetEmpty());

            BigDouble totalDamage = BigDouble.Max(1, dmg.Select(x => x.Value).Sum());

            for (int i = 0; i < Mathf.Max(dmg.Count, MercSlots.Count); ++i)
            {
                if (i >= MercSlots.Count)
                    InstantiateMercSummarySlot();

                MercDamageSummarySlot slot = MercSlots[i];

                MercID mercId = dmg[i].Key;
                BigDouble damageDealt = dmg[i].Value;

                float percent = (float)(damageDealt / totalDamage).ToDouble();

                slot.UpdateValues(mercId, damageDealt, percent);
            }
        }

        private void InstantiateMercSummarySlot()
        {
            GameObject go = Instantiate(MercSummarySlotObject, MercSlotsParent);

            MercDamageSummarySlot slot = go.GetComponent<MercDamageSummarySlot>();

            MercSlots.Add(slot);
        }
    }
}