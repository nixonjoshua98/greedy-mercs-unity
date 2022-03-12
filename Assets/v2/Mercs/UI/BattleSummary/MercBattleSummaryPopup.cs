using GM.Common.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GM.Mercs.UI
{
    public class MercBattleSummaryPopup : MonoBehaviour
    {
        [SerializeField] List<MercDamageSummarySlot> DamageSlots;

        public void UpdateDamageNumbers(List<KeyValuePair<MercID, BigDouble>> dmg)
        {
            DamageSlots.ForEach(slot => slot.SetEmpty());

            BigDouble totalDamage = BigDouble.Max(1, dmg.Select(x => x.Value).Sum());

            for (int i = 0; i < Mathf.Min(dmg.Count, DamageSlots.Count); ++i)
            {
                MercDamageSummarySlot slot = DamageSlots[i];

                MercID mercId = dmg[i].Key;
                BigDouble damageDealt = dmg[i].Value;

                float percent = (float)(damageDealt / totalDamage).ToDouble();

                slot.UpdateValues(mercId, damageDealt, percent);
            }
        }
    }
}