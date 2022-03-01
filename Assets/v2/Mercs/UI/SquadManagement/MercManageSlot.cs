using GM.Common.Enums;
using GM.Mercs.Data;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Mercs.UI
{
    public class MercManageSlot : GM.Core.GMMonoBehaviour
    {
        [Header("Sprites")]
        [SerializeField] Sprite InSquadBackgroundSprite;
        [SerializeField] Sprite OutSquadBackgroundSprite;

        [Header("References")]
        public GameObject AddButton;
        public GameObject RemoveButton;
        public GameObject NeutrelButton;

        public Image BackgroundImage;
        public Image MercIconImage;

        MercManagePopup Manager;
        MercData MercData;

        public bool InSquad { get; private set; } = false;

        public void Set(MercManagePopup manager, UnitID unit)
        {
            Manager = manager;
            MercData = App.Data.Mercs.GetMerc(unit);
            InSquad = MercData.InDefaultSquad;

            SetInitialUI();
        }

        void SetInitialUI()
        {
            MercIconImage.sprite = MercData.Icon;
            UpdateActiveUI();
        }

        public void UpdateActiveUI()
        {
            AddButton.SetActive(!InSquad && !Manager.SquadFull);
            RemoveButton.SetActive(InSquad);
            NeutrelButton.SetActive(!InSquad && Manager.SquadFull);

            BackgroundImage.sprite = InSquad ? InSquadBackgroundSprite : OutSquadBackgroundSprite;
        }

        // = UI Callbacks = //

        public void Button_AddMerc()
        {
            InSquad = true;
            Manager.UpdateMerc(MercData.ID, true);
            UpdateActiveUI();
        }

        public void Button_RemoveMerc()
        {
            InSquad = false;
            Manager.UpdateMerc(MercData.ID, false);
            UpdateActiveUI();
        }
    }
}
