
using UnityEngine;
using UnityEngine.UI;

namespace GM.Artefacts
{
    using GM.UI;
    using GM.Data;

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
        [SerializeField] Text levelText;
        [SerializeField] Text effectText;
        
        [Header("Components - Scripts")]
        [SerializeField] StackedButton stackedButton;

        int _buyAmount;
        bool _updatingUi;

        int BuyAmount
        { 
            get 
            {
                ArtefactData artData = GameData.Get().Artefacts.Get(_artefactId);
                ArtefactState artState = ArtefactManager.Instance.Get(_artefactId);

                return Mathf.Min(_buyAmount, artData.MaxLevel - artState.Level); 
            } 
        }

        void Awake()
        {
            BuyController buyController = FindObjectOfType<LootTab>().GetComponentInChildren<BuyController>();

            buyController.AddListener((val) => { _buyAmount = val; });
        }


        public void Init(int id)
        {
            _artefactId = id;
            _updatingUi = true;

            UpdateInterfacElements();
        }


        protected override void PeriodicUpdate()
        {
            if (_updatingUi)
            {
                UpdateInterfacElements();
            }
        }


        void UpdateInterfacElements()
        {
            ArtefactData artData = GameData.Get().Artefacts.Get(_artefactId);
            ArtefactState artState = ArtefactManager.Instance.Get(_artefactId);

            int pp = UserData.Get().Inventory.PrestigePoints;

            icon.sprite = artData.Icon;

            levelText.text  = $"Lvl. {artState.Level}";
            effectText.text = $"{FormatString.Number(artState.Effect())} {artData.Bonus}";
            nameText.text   = artData.Name;

            stackedButton.SetText("MAX", "-");

            if (!artState.IsMaxLevel())
            {
                string cost = FormatString.Number(artState.CostToUpgrade(BuyAmount));

                stackedButton.SetText(string.Format("x{0}", BuyAmount), cost);
            }

            buyButton.interactable = !artState.IsMaxLevel() && pp >= artState.CostToUpgrade(BuyAmount);
        }


        // = = = Button Callbacks = = = //
        public void OnUpgradeArtefactBtn()
        {
            ArtefactManager.Instance.UpgradeArtefact(_artefactId, BuyAmount, (_) => { });
        }
    }
}