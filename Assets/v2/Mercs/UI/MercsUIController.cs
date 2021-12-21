using UnityEngine;
using MercID = GM.Common.Enums.MercID;

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
            App.Events.MercUnlocked.AddListener(OnMercUnlocked);
        }

        void FixedUpdate()
        {
            UpdateUnlockButton();
        }

        void UpdateUnlockButton()
        {
            MercUnlockButton.SetText("UNLOCKED", "");
            MercUnlockButton.interactable = false;

            if (App.Data.Mercs.GetNextHero(out MercID chara))
            {
                GM.Mercs.Models.MercGameDataModel mercData = App.Data.Mercs.GetGameMerc(chara);

                MercUnlockButton.SetText("UNLOCK", Format.Number(mercData.UnlockCost));

                MercUnlockButton.interactable = App.Data.Inv.Gold >= mercData.UnlockCost;
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
