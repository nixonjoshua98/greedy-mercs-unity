using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GM.Mercs.UI
{
    public class MercPopup : MercUIObject
    {
        public TMP_Text PassiveText;

        protected override void OnAssigned()
        {
            List<Models.MercPassiveDataModel> passives = AssignedMerc.UnlockedPassives;

            foreach (Models.MercPassiveDataModel passive in passives)
            {
                PassiveText.text += $"{Format.Bonus(passive.Type, passive.Value)}\n";
            }
        }
    }
}
