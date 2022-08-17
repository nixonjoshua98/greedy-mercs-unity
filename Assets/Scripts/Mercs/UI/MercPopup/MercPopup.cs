using SRC.Mercs.Data;
using TMPro;
using UnityEngine;

namespace SRC.Mercs.UI
{
    public class MercPopup : SRC.UI.PopupBase
    {
        private MercID _mercID;

        [Header("Prefabs")]
        [SerializeField] private GameObject PassiveSlotObject;

        [Header("Text Elements")]
        [SerializeField] private TMP_Text NameText;
        [SerializeField] private TMP_Text LevelText;
        [SerializeField] private TMP_Text DamageText;

        [Header("Transforms")]
        [SerializeField] private Transform PassivesParent;

        [Space]

        [SerializeField] private SRC.UI.GenericGradeItem GradeSlot;

        private AggregatedMercData Merc { get => App.Mercs.GetMerc(_mercID); }

        public void Initialize(AggregatedMercData merc)
        {
            _mercID = merc.MercID;

            InstantiatePassiveSlots();

            UpdateUI();

            ShowInnerPanel();
        }

        private void UpdateUI()
        {
            NameText.text = Merc.Name;
            DamageText.text = $"{Format.Number(Merc.DamagePerAttack)}";
            LevelText.text = $"{Merc.CurrentLevel}";

            GradeSlot.Intialize(Merc);
        }

        private void InstantiatePassiveSlots()
        {
            Merc.Passives.ForEach(passive =>
            {
                var slot = this.Instantiate<MercPassiveSlot>(PassiveSlotObject, PassivesParent);

                slot.Initialize(passive);
            });
        }

        /* Event Listeners */

        public void OnCloseButton()
        {
            Destroy(gameObject);
        }
    }
}