using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.Characters
{
    using GM.Events;

    using Utils = GM.Utils;

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

        CharacterID _mercId;

        int _buyAmount;
        bool _updatingUi;

        protected MercState State { get { return MercenaryManager.Instance.GetState(_mercId); } }
        protected MercData Data { get { return StaticData.Mercs.GetMerc(_mercId); } }

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

        public void SetCharacter(CharacterID chara)
        {
            _mercId = chara;

            nameText.text = Data.Name;
            iconImage.sprite = Data.Icon;

            _updatingUi = true;
        }
        
        protected override void PeriodicUpdate()
        {
            if (!_updatingUi)
                return;

            DamageText.text = Utils.Format.FormatNumber(StatsCache.TotalMercDamage(_mercId)) + " DPS";
            nameText.text   = string.Format("(Lvl. {0}) {1}", State.Level, Data.Name);

            upgradeButton.SetText("MAX", "-");

            if (State.Level < StaticData.MAX_CHAR_LEVEL)
            {
                BigDouble cost = State.CostToUpgrade(TargetBuyAmount);

                upgradeButton.SetText(string.Format("x{0}", TargetBuyAmount), Utils.Format.FormatNumber(cost));
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

                GlobalEvents.OnCharacterLevelUp.Invoke(_mercId);
            }
        }

        public void OnShowInfo()
        {
            GameObject panel = Utils.UI.Instantiate(CharacterPanelObject, Vector3.zero);

            panel.GetComponent<CharacterPanel>().SetHero(_mercId);
        }
    }
}