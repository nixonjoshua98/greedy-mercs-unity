using GM.Common.Enums;
using GM.Mercs.Data;
using TMPro;
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
        public TMP_Text LevelText;
        public TMP_Text NameText;
        [SerializeField] private TMP_Text EnergyText;

        public MercID MercID { get; private set; }

        private MercManagePopup Manager;
        private AggregatedMercData MercData;

        public bool InSquad { get; private set; } = false;

        public void Set(MercManagePopup manager, MercID unit)
        {
            Manager = manager;
            MercID = unit;
            MercData = App.Mercs.GetMerc(unit);
            InSquad = MercData.InSquad;

            NameText.text = MercData.Name;
            EnergyText.text = $"{MercData.RechargeRate}s";
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
