using UnityEngine;

namespace GM.Mercs.UI
{
    public class MercsUIController : GM.UI.Panels.Panel
    {
        [Header("References")]
        public Transform SlotsParent;

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

            slot.Assign(merc);
        }


        protected override void OnShown()
        {

        }

        protected override void OnHidden()
        {

        }
    }
}
