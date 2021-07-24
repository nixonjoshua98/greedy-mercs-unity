using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.Units
{
    using GM.Data;

    using GM.Events;

    using GM;
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

        public Data.MercData _MercDescription { get { return GameData.Get().Mercs.Get(_mercId); } }

        void Awake()
        {
            BuyController buyController = FindObjectOfType<MercsTab>().GetComponentInChildren<BuyController>();

            buyController.AddListener((val) => { _buyAmount = val; });
        }

        protected int TargetBuyAmount
        {
            get
            {
                if (_buyAmount == -1)
                    return Formulas.AffordCharacterLevels(_mercId);

                return Mathf.Min(_buyAmount, StaticData.MAX_CHAR_LEVEL - State.Level);
            }
        }

        public void SetCharacter(MercID mercId)
        {
            _mercId = mercId;

            nameText.text       = _MercDescription.Name;
            iconImage.sprite    = _MercDescription.Icon;

            _updatingUi = true;
        }
        
        protected override void PeriodicUpdate()
        {
            if (!_updatingUi)
                return;

            DamageText.text = FormatString.Number(StatsCache.TotalMercDamage(_mercId)) + " DPS";
            nameText.text   = string.Format("(Lvl. {0}) {1}", State.Level, _MercDescription.Name);

            upgradeButton.SetText("MAX", "-");

            if (State.Level < StaticData.MAX_CHAR_LEVEL)
            {
                BigDouble cost = State.CostToUpgrade(TargetBuyAmount);

                upgradeButton.SetText(string.Format("x{0}", TargetBuyAmount), FormatString.Number(cost));
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