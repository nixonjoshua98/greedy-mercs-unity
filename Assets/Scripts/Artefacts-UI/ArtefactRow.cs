
using UnityEngine;
using UnityEngine.UI;

namespace GM.Artefacts
{
    using GM.UI;
    using GM.Inventory;

    using Utils = GM.Utils;

    public class ArtefactRow : ExtendedMonoBehaviour
    {
        int _artefactId;

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

        ArtefactData ServerData { get { return StaticData.Artefacts.Get(_artefactId); } }
        ArtefactState ArtState { get { return ArtefactManager.Instance.Get(_artefactId); } }

        int BuyAmount { get {  return Mathf.Min(_buyAmount, ServerData.MaxLevel - ArtState.Level); } }

        void Awake()
        {
            BuyController buyController = FindObjectOfType<LootTab>().GetComponentInChildren<BuyController>();

            buyController.AddListener((val) => { _buyAmount = val; });
        }

        public void Init(int id)
        {
            _artefactId = id;

            nameText.text = ServerData.Name;
            icon.sprite = ServerData.Icon;

            _updatingUi = true;
        }

        protected override void PeriodicUpdate()
        {
            if (!_updatingUi)
                return;

            InventoryManager inv = InventoryManager.Instance;

            UpdateEffectText();

            nameText.text = string.Format("(Lvl. {0}) {1}", ArtState.Level, ServerData.Name);

            stackedButton.SetText("MAX", "-");

            if (!ArtState.IsMaxLevel())
            {
                string cost = FormatString.Number(ArtState.CostToUpgrade(BuyAmount));

                stackedButton.SetText(string.Format("x{0}", BuyAmount), cost);
            }

            buyButton.interactable = !ArtState.IsMaxLevel() && inv.PrestigePoints >= ArtState.CostToUpgrade(BuyAmount);
        }

        void UpdateEffectText()
        {
            effectText.text = string.Format("{0} {1}", FormatString.Number(ArtState.Effect()), Funcs.BonusString(ServerData.bonusType));
        }

        // = = = Button Callbacks = = = //
        public void OnUpgradeArtefactBtn()
        {
            void ServerCallback(bool purchased)
            {

            }

            ArtefactManager.Instance.UpgradeArtefact(_artefactId, BuyAmount, ServerCallback);
        }
    }
}