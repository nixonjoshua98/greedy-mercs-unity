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
        ArtefactState State { get { return ArtefactManager.Instance.Get(artefactId); } }

        int BuyAmount
        {
            get
            {
                return Mathf.Min(_buyAmount, ServerData.MaxLevel - State.Level);
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

            nameText.text = string.Format("(Lvl. {0}) {1}", State.Level, ServerData.Name);

            stackedButton.SetText("MAX", "-");

            if (State.Level < ServerData.MaxLevel)
            {
                string cost = Utils.Format.FormatNumber(State.CostToUpgrade(BuyAmount));

                stackedButton.SetText(string.Format("x{0}", BuyAmount), cost);
            }


            buyButton.interactable = State.Level < ServerData.MaxLevel && inv.PrestigePoints >= State.CostToUpgrade(BuyAmount);
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
            void ServerCallback(bool purchased)
            {

            }

            ArtefactManager.Instance.UpgradeArtefact(artefactId, BuyAmount, ServerCallback);
        }
    }
}