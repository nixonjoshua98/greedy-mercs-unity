using SRC.GoldUpgrades.UI;
using UnityEngine;

namespace SRC.UI
{
    public class PlayerTabController : SRC.Core.GMMonoBehaviour
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
            this.InstantiateUI<SRC.Quests.UI.QuestsPopup>(QuestsPopupObject);
        }
    }
}
