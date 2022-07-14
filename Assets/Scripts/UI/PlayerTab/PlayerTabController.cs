using GM.GoldUpgrades.UI;
using UnityEngine;

namespace GM.UI
{
    public class PlayerTabController : GM.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject QuestsPopupObject;

        [Header("References")]
        //[SerializeField] private AmountSelector UpgradeAmountSelector;
        [Space]
        [SerializeField] private TapDamageGoldUpgradeSlot TapUpgradeSlot;

        private void Start()
        {
            //UpgradeAmountSelector.E_OnChange.AddListener(TapUpgradeSlot.AmountSelector_ValueChanged);

            //UpgradeAmountSelector.InvokeChangeEvent();
        }

        public void OpenQuestsPopup()
        {
            this.InstantiateUI<GM.Quests.UI.QuestsPopup>(QuestsPopupObject);
        }
    }
}
