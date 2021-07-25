
using UnityEngine;
using UnityEngine.UI;

namespace GM.Artefacts
{
    using GM.UI;
    using GM.Data;

    public class ArtefactSlot : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Image icon;
        [Space]
        [SerializeField] Text nameText;
        [SerializeField] Text levelText;
        [SerializeField] Text effectText;
        
        [Header("Components - Scripts")]
        [SerializeField] StackedButton stackedButton;

        int _artefactId;
        int _buyAmount;
        bool _updatingUi;

        int BuyAmount
        { 
            get 
            {
                ArtefactData artData    = GameData.Get().Artefacts.Get(_artefactId);
                ArtefactState artState  = ArtefactManager.Instance.Get(_artefactId);

                return MathUtils.NextMultipleMax(artState.Level, _buyAmount, artData.MaxLevel);
            } 
        }

        void OnEnable()
        {
            UpdateInterfacElements();
        }


        public void Init(int id, BuyController buyController)
        {
            _artefactId = id;
            _updatingUi = true;

            buyController.AddListener((val) => {
                _buyAmount = val;

                UpdateInterfacElements();
            });

            UpdateInterfacElements();
        }


        void UpdateInterfacElements()
        {
            if (!_updatingUi)
                return;

            ArtefactData artData = GameData.Get().Artefacts.Get(_artefactId);
            ArtefactState artState = ArtefactManager.Instance.Get(_artefactId);

            int pp = UserData.Get().Inventory.PrestigePoints;

            icon.sprite = artData.Icon;

            levelText.text  = $"Lvl. {artState.Level}";
            effectText.text = FormatString.Bonus(artData.Bonus, artState.Effect());
            nameText.text   = artData.Name;

            stackedButton.SetText("MAX", "-");

            if (!artState.IsMaxLevel())
            {
                string cost = FormatString.Number(artState.CostToUpgrade(BuyAmount));

                stackedButton.SetText(string.Format("x{0}", BuyAmount), cost);
            }

            stackedButton.Toggle(!artState.IsMaxLevel() && pp >= artState.CostToUpgrade(BuyAmount));
        }


        // = = = Button Callbacks = = = //
        public void OnUpgradeArtefactBtn()
        {
            ArtefactManager.Instance.UpgradeArtefact(_artefactId, BuyAmount, (_) => { UpdateInterfacElements(); });
        }
    }
}