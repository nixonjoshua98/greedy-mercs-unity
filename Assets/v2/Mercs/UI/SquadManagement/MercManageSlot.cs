using GM.Common.Enums;
using GM.Mercs.Data;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Mercs.UI
{
    public class MercManageSlot : GM.Core.GMMonoBehaviour
    {
        [Header("References")]
        public GameObject AddButton;
        public GameObject RemoveButton;
        public GameObject NeutrelButton;
        [Space]
        public Image MercIconImage;

        public UnitID Unit { get; private set; }

        MercManagePopup Manager;
        MercData MercData;

        public bool InSquad { get; private set; } = false;

        public void Set(MercManagePopup manager, UnitID unit)
        {
            Manager = manager;
            Unit = unit;
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
        }

        // = UI Callbacks = //

        public void Button_AddMerc()
        {
            InSquad = true;
            Manager.UpdateMerc(MercData.ID);
            UpdateActiveUI();
        }

        public void Button_RemoveMerc()
        {
            InSquad = false;
            Manager.UpdateMerc(MercData.ID);
            UpdateActiveUI();
        }
    }
}
