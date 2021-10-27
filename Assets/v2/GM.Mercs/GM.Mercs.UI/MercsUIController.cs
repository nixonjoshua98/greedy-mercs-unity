using UnityEngine;
using AmountSelector = GM.UI.AmountSelector;

namespace GM.Mercs.UI
{
    public class MercsUIController : GM.UI.Panels.Panel
    {
        [Header("References")]
        public Transform SlotsParent;
        public AmountSelector UpgradeAmountSelector;

        [Header("Prefabs")]
        public GameObject SlotObject;

        void Awake()
        {
            App.Data.Mercs.E_MercUnlocked.AddListener(OnMercUnlocked);
        }

        // == Callbacks == //
        void OnMercUnlocked(MercID merc)
        {
            var slot = Instantiate<MercSlot>(SlotObject, SlotsParent);

            slot.Assign(merc, UpgradeAmountSelector);
        }


        protected override void OnShown()
        {

        }

        protected override void OnHidden()
        {

        }
    }
}
