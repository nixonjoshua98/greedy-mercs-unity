using GM.Common.Enums;
using GM.Mercs.Data;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        public TMP_Text LevelText;
        public TMP_Text NameText;

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

            NameText.text = MercData.Name;
            LevelText.text = $"Lvl. <color=orange>{MercData.CurrentLevel}</color>";
            MercIconImage.sprite = MercData.Icon;
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
