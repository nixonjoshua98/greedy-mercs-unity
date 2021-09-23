
using UnityEngine;
using UnityEngine.UI;

namespace GM.Units
{
    using GM.Data;
    using GM.Events;
    using GM.UI;

    public class CharacterRow : ExtendedMonoBehaviour
    {
        [SerializeField] StackedButton upgradeButton;

        [Header("Images")]
        [SerializeField] Image iconImage;

        [Header("Text")]
        [SerializeField] Text nameText;
        [SerializeField] Text DamageText;

        [Header("Prefabs")]
        [SerializeField] GameObject CharacterPanelObject;

        GM.Mercs.Data.FullMercData mercData;

        int _buyAmount;

        protected MercState State { get { return MercenaryManager.Instance.GetState(mercData.ID); } }

        protected int BuyAmount
        {
            get
            {
                if (_buyAmount == -1)
                    return Formulas.AffordCharacterLevels(mercData.ID);

                return Mathf.Min(_buyAmount, global::Constants.MAX_CHAR_LEVEL - State.Level);
            }
        }


        public void Setup(MercID merc, BuyController buyController)
        {
            mercData = App.Data.Mercs.GetMerc(merc);

            buyController.AddListener((val) => { _buyAmount = val; });

            SetInterfaceElements();
            UpdateInterfaceElements();
        }
        

        protected override void PeriodicUpdate()
        {
            UpdateInterfaceElements();
        }


        void SetInterfaceElements()
        {
            iconImage.sprite = mercData.GameValues.Icon;
        }


        void UpdateInterfaceElements()
        {
            DamageText.text = FormatString.Number(StatsCache.TotalMercDamage(mercData.ID), prefix: " ATK");
            nameText.text   = $"(Lvl. {State.Level}) {mercData.GameValues.Name}";

            upgradeButton.SetText("MAX", "-");

            if (State.Level < global::Constants.MAX_CHAR_LEVEL)
            {
                BigDouble cost = State.CostToUpgrade(BuyAmount);

                upgradeButton.SetText($"x{BuyAmount}", FormatString.Number(cost));
            }
        }


        // === Button Callbacks === //

        public void OnUpgrade()
        {
            int levelsBuying = BuyAmount;

            BigDouble cost = State.CostToUpgrade(BuyAmount);

            if (State.Level + levelsBuying <= global::Constants.MAX_CHAR_LEVEL && UserData.Get.Inventory.Gold >= cost)
            {
                MercenaryManager.Instance.AddLevels(mercData.ID, levelsBuying);

                UserData.Get.Inventory.Gold -= cost;

                GlobalEvents.E_OnMercLevelUp.Invoke(mercData.ID);
            }
        }


        public void OnShowInfo()
        {
            GameObject panel = CanvasUtils.Instantiate(CharacterPanelObject);

            panel.GetComponent<CharacterPanel>().SetHero(mercData.ID);
        }
    }
}