using GM.Mercs.Data;
using TMPro;
using UnityEngine;

namespace GM.Mercs.UI
{
    public class MercPopup : GM.UI.PopupBase
    {
        private MercID _mercID;

        [Header("Prefabs")]
        [SerializeField] GameObject PassiveSlotObject;

        [Header("Text Elements")]
        [SerializeField] TMP_Text NameText;
        [SerializeField] TMP_Text LevelText;
        [SerializeField] TMP_Text DamageText;

        [Header("Transforms")]
        [SerializeField] Transform PassivesParent;

        [Space]

        [SerializeField] GM.UI.GenericGradeItem GradeSlot;

        AggregatedMercData Merc { get => App.Mercs.GetMerc(_mercID); }

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

        void InstantiatePassiveSlots()
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