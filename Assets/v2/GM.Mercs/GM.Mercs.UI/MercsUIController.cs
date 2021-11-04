using UnityEngine;

namespace GM.Mercs.UI
{
    public class MercsUIController : GM.UI.Panels.TogglablePanel
    {
        [Header("References")]
        public Transform SlotsParent;
        public GM.UI.AmountSelector UpgradeAmountSelector;
        public GM.UI.VStackedButton MercUnlockButton;

        [Header("Prefabs")]
        public GameObject SlotObject;

        void Awake()
        {
            App.Data.Mercs.E_MercUnlocked.AddListener(OnMercUnlocked);
        }

        void FixedUpdate()
        {
            UpdateUnlockButton();
        }

        void UpdateUnlockButton()
        {
            MercUnlockButton.SetText("UNLOCKED", "");

            if (App.Data.Mercs.GetNextHero(out MercID chara))
            {
                GM.Mercs.Models.MercGameDataModel mercData = App.Data.Mercs.GetGameMerc(chara);

                MercUnlockButton.SetText("UNLOCK", Format.Number(mercData.UnlockCost));
            }
        }

        // == Callbacks == //
        void OnMercUnlocked(MercID merc)
        {
            var slot = Instantiate<MercSlot>(SlotObject, SlotsParent);

            slot.Assign(merc, UpgradeAmountSelector);
        }

        public void OnUnlockButton()
        {
            if (App.Data.Mercs.GetNextHero(out MercID chara))
            {
                GM.Mercs.Models.MercGameDataModel mercData = App.Data.Mercs.GetGameMerc(chara);

                if (App.Data.Inv.Gold >= mercData.UnlockCost)
                {
                    App.Data.Inv.Gold -= mercData.UnlockCost;

                    App.Data.Mercs.UnlockUserMerc(chara);
                }
            }
        }
    }
}
