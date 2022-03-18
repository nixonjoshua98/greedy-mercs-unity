using GM.GoldUpgrades.UI;
using UnityEngine;

namespace GM.UI
{
    public class PlayerTabController : GM.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] GameObject QuestsPopupObject;

        [Header("References")]
        [SerializeField] AmountSelector UpgradeAmountSelector;
        [Space]
        [SerializeField] TapDamageGoldUpgradeSlot TapUpgradeSlot;


        void Start()
        {
            UpgradeAmountSelector.E_OnChange.AddListener(TapUpgradeSlot.AmountSelector_ValueChanged);

            UpgradeAmountSelector.InvokeChangeEvent();
        }

        public void OpenQuestsPopup()
        {
            InstantiateUI<GM.Quests.UI.QuestsPopup>(QuestsPopupObject);
        }
    }
}
