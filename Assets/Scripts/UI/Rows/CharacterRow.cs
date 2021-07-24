
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

        MercID _mercId;

        int _buyAmount;
        bool _updatingUi;

        protected MercState State { get { return MercenaryManager.Instance.GetState(_mercId); } }

        protected int TargetBuyAmount
        {
            get
            {
                if (_buyAmount == -1)
                    return Formulas.AffordCharacterLevels(_mercId);

                return Mathf.Min(_buyAmount, StaticData.MAX_CHAR_LEVEL - State.Level);
            }
        }

        public void Setup(MercID mercId, BuyController buyController)
        {
            _mercId = mercId;
            _updatingUi = true;

            buyController.AddListener((val) => { _buyAmount = val; });

            SetInterfaceElements();
            UpdateInterfaceElements();
        }
        

        protected override void PeriodicUpdate()
        {
            if (_updatingUi)
                UpdateInterfaceElements();
        }


        void SetInterfaceElements()
        {
            MercData mercData = GameData.Get().Mercs.Get(_mercId);

            iconImage.sprite = mercData.Icon;
        }


        void UpdateInterfaceElements()
        {
            MercData mercData = GameData.Get().Mercs.Get(_mercId);

            DamageText.text = FormatString.Number(StatsCache.TotalMercDamage(_mercId), prefix: " ATK");
            nameText.text   = string.Format("(Lvl. {0}) {1}", State.Level, mercData.Name);

            upgradeButton.SetText("MAX", "-");

            if (State.Level < StaticData.MAX_CHAR_LEVEL)
            {
                BigDouble cost = State.CostToUpgrade(TargetBuyAmount);

                upgradeButton.SetText($"x{TargetBuyAmount}", FormatString.Number(cost));
            }
        }


        // === Button Callbacks ===

        public void OnUpgrade()
        {
            int levelsBuying = TargetBuyAmount;

            BigDouble cost = State.CostToUpgrade(TargetBuyAmount);

            if (State.Level + levelsBuying <= StaticData.MAX_CHAR_LEVEL && GameState.Player.gold >= cost)
            {
                MercenaryManager.Instance.AddLevels(_mercId, levelsBuying);

                GameState.Player.gold -= cost;

                GlobalEvents.E_OnMercLevelUp.Invoke(_mercId);
            }
        }


        public void OnShowInfo()
        {
            GameObject panel = CanvasUtils.Instantiate(CharacterPanelObject);

            panel.GetComponent<CharacterPanel>().SetHero(_mercId);
        }
    }
}