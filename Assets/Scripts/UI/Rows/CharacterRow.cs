
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

        MercData mercData;

        int _buyAmount;

        protected MercState State { get { return MercenaryManager.Instance.GetState(mercData.Id); } }

        protected int BuyAmount
        {
            get
            {
                if (_buyAmount == -1)
                    return Formulas.AffordCharacterLevels(mercData.Id);

                return Mathf.Min(_buyAmount, StaticData.MAX_CHAR_LEVEL - State.Level);
            }
        }


        public void Setup(MercID merc, BuyController buyController)
        {
            mercData = GameData.Get.Mercs.Get(merc);

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
            iconImage.sprite = mercData.Icon;
        }


        void UpdateInterfaceElements()
        {
            DamageText.text = FormatString.Number(StatsCache.TotalMercDamage(mercData.Id), prefix: " ATK");
            nameText.text   = $"(Lvl. {State.Level}) {mercData.Name}";

            upgradeButton.SetText("MAX", "-");

            if (State.Level < StaticData.MAX_CHAR_LEVEL)
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

            if (State.Level + levelsBuying <= StaticData.MAX_CHAR_LEVEL && GameState.Player.gold >= cost)
            {
                MercenaryManager.Instance.AddLevels(mercData.Id, levelsBuying);

                GameState.Player.gold -= cost;

                GlobalEvents.E_OnMercLevelUp.Invoke(mercData.Id);
            }
        }


        public void OnShowInfo()
        {
            GameObject panel = CanvasUtils.Instantiate(CharacterPanelObject);

            panel.GetComponent<CharacterPanel>().SetHero(mercData.Id);
        }
    }
}