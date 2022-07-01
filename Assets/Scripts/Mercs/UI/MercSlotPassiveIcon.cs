using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GM.Mercs.Data;
using GM.Common;

namespace GM.Mercs.UI
{
    public class MercSlotPassiveIcon : GM.Core.GMMonoBehaviour
    {
        [Header("Sprites")]
        [SerializeField] Sprite UnlockedSprite;
        [Space]
        [SerializeField] Image BackgroundImage;
        [SerializeField] Image BorderImage;
        [Space]
        [SerializeField] TMP_Text LevelText;

        AggregatedMercData Merc;
        int PassiveID;
        MercPassive Passive => Merc.Passives.Find(x => x.PassiveID == PassiveID);

        public void Initialize(AggregatedMercData merc, int passiveId)
        {
            Merc = merc;
            PassiveID = passiveId;

            LevelText.text = Passive.UnlockLevel.ToString();

            InvokeRepeating(nameof(CheckForUnlock), 0.0f, 0.5f);
        }

        void CheckForUnlock()
        {
            if (Merc.IsPassiveUnlocked(PassiveID))
            {
                BackgroundImage.sprite = UnlockedSprite;
                BorderImage.color = Constants.Colors.Purple;

                CancelInvoke(nameof(CheckForUnlock));
            }
        }
    }
}
