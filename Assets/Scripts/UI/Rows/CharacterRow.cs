
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

        MercID _MercID;

        int _buyAmount;

        protected GM.Mercs.Data.FullMercData MercData => App.Data.Mercs[_MercID];

        protected int BuyAmount
        {
            get
            {
                if (_buyAmount == -1)
                    return Formulas.AffordCharacterLevels(_MercID);

                return Mathf.Min(_buyAmount, global::Constants.MAX_CHAR_LEVEL - MercData.User.Level);
            }
        }


        public void Setup(MercID merc, BuyController buyController)
        {
            _MercID = merc;

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
            iconImage.sprite = MercData.GameData.Icon;
        }


        void UpdateInterfaceElements()
        {
            DamageText.text = FormatString.Number(StatsCache.TotalMercDamage(_MercID), prefix: " ATK");
            nameText.text   = $"(Lvl. {MercData.User.Level}) {MercData.GameData.Name}";

            upgradeButton.SetText("MAX", "-");

            if (MercData.User.Level < global::Constants.MAX_CHAR_LEVEL)
            {
                BigDouble cost = MercData.CostToUpgrade(BuyAmount);

                upgradeButton.SetText($"x{BuyAmount}", FormatString.Number(cost));
            }
        }


        // === Button Callbacks === //

        public void OnUpgrade()
        {
            int levelsBuying = BuyAmount;

            BigDouble cost = MercData.CostToUpgrade(BuyAmount);

            if (MercData.User.Level + levelsBuying <= global::Constants.MAX_CHAR_LEVEL && UserData.Get.Inventory.Gold >= cost)
            {
                App.Data.Mercs.User.AddLevels(_MercID, levelsBuying);

                UserData.Get.Inventory.Gold -= cost;

                GlobalEvents.E_OnMercLevelUp.Invoke(_MercID);
            }
        }


        public void OnShowInfo()
        {
            GameObject panel = CanvasUtils.Instantiate(CharacterPanelObject);

            panel.GetComponent<CharacterPanel>().SetHero(_MercID);
        }
    }
}