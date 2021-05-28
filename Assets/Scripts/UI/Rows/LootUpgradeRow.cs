using System.Numerics;

using UnityEngine;
using UnityEngine.UI;

using SimpleJSON;

namespace GreedyMercs
{
    using GM.UI;
    using GM.Artefacts;
    using GM.Inventory;

    public class LootUpgradeRow : MonoBehaviour
    {
        int artefactId;

        [Header("Components")]
        [SerializeField] Image icon;
        [Space]
        [SerializeField] Button buyButton;
        [Space]
        [SerializeField] Text nameText;
        [SerializeField] Text effectText;
        
        [Header("Components - Scripts")]
        [SerializeField] StackedButton stackedButton;

        int _buyAmount;
        bool _updatingUi;

        ArtefactData ServerData { get { return StaticData.Artefacts.Get(artefactId); } }
        ArtefactState ArtefactState { get { return ArtefactManager.Instance.Get(artefactId); } }

        int BuyAmount
        {
            get
            {
                return Mathf.Min(_buyAmount, ServerData.MaxLevel - ArtefactState.Level);
            }
        }

        void Awake()
        {
            BuyController buyController = FindObjectOfType<LootTab>().GetComponentInChildren<BuyController>();

            buyController.AddListener((val) => { _buyAmount = val; });
        }

        public void Init(int id)
        {
            artefactId = id;

            nameText.text = ServerData.Name;
            icon.sprite = ServerData.Icon;

            _updatingUi = true;
        }

        void FixedUpdate()
        {
            if (!_updatingUi)
                return;

            InventoryManager inv = InventoryManager.Instance;

            UpdateEffectText();

            nameText.text = string.Format("(Lvl. {0}) {1}", ArtefactState.Level, ServerData.Name);

            stackedButton.SetText("MAX", "-");

            if (ArtefactState.Level < ServerData.MaxLevel)
            {
                string cost = Utils.Format.FormatNumber(Formulas.CalcLootItemLevelUpCost(artefactId, BuyAmount));

                stackedButton.SetText(string.Format("x{0}", BuyAmount), cost);
            }


            buyButton.interactable = ArtefactState.Level < ServerData.MaxLevel && inv.prestigePoints >= Formulas.CalcLootItemLevelUpCost(artefactId, BuyAmount);
        }

        void UpdateEffectText()
        {
            double effect = Formulas.CalcLootItemEffect(artefactId);

            switch (ServerData.valueType)
            {
                case ValueType.MULTIPLY:
                    effectText.text = Utils.Format.FormatNumber(effect * 100) + "%";
                    break;

                case ValueType.ADDITIVE_PERCENT:
                    effectText.text = "+ " + Utils.Format.FormatNumber(effect * 100) + "%";
                    break;

                case ValueType.ADDITIVE_FLAT_VAL:
                    effectText.text = "+ " + Utils.Format.FormatNumber(effect);
                    break;
            }

            effectText.text += " " + Utils.Generic.BonusToString(ServerData.bonusType);
        }

        // = = = Button Callbacks = = =

        public void OnUpgradeArtefactBtn()
        {
            InventoryManager inv = InventoryManager.Instance;

            int levelsBuying = BuyAmount;

            BigInteger cost = Formulas.CalcLootItemLevelUpCost(artefactId, levelsBuying);

            void ServerCallback(long code, string compressed) => OnUpgradeCallback(levelsBuying, code, compressed);

            if (levelsBuying > 0 && inv.prestigePoints >= cost && (ArtefactState.Level + levelsBuying) <= ServerData.MaxLevel)
            {
                JSONNode node = Utils.Json.GetDeviceInfo();

                node.Add("itemId", artefactId);
                node.Add("buyLevels", BuyAmount);

                Server.UpgradeLootItem(ServerCallback, node);
            }
        }

        void OnUpgradeCallback(int levelsBuying, long code, string compressed)
        {
            InventoryManager inv = InventoryManager.Instance;

            if (code == 200)
            {
                JSONNode node = Utils.Json.Decompress(compressed);

                ArtefactState.Level += levelsBuying;

                inv.prestigePoints = node["remainingPoints"].AsInt;
            }
        }
    }
}