
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Artefacts
{
    using GM.UI;

    public class ArtefactSlot : Core.GMMonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Image icon;
        [Space]
        [SerializeField] TMPro.TMP_Text nameText;
        [SerializeField] TMPro.TMP_Text levelText;
        [SerializeField] Text effectText;
        [SerializeField] Text purchaseText;
        [Space]        
        [SerializeField] Button upgradeButton;

        int _artefactId;
        int _buyAmount;
        bool _updatingUi;

        Data.Artefacts.FullArtefactData ArtefactItem => App.Data.Arts.GetArtefact(_artefactId);

        int BuyAmount => MathUtils.NextMultipleMax(ArtefactItem.State.Level, _buyAmount, ArtefactItem.Values.MaxLevel);

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

            SetInterfaceElements();

            UpdateInterfacElements();
        }


        void SetInterfaceElements()
        {
            nameText.text = ArtefactItem.Values.Name;

            icon.sprite = ArtefactItem.Values.Icon;
        }


        void UpdateInterfacElements()
        {
            if (!_updatingUi)
                return;
            
            BigInteger pp = UserData.Get.Inventory.PrestigePoints;

            levelText.text  = $"Lvl. {ArtefactItem.State.Level}";
            effectText.text = FormatString.Bonus(ArtefactItem.Values.Bonus, ArtefactItem.BaseEffect);

            purchaseText.text = "-";

            if (!ArtefactItem.IsMaxLevel)
            {
                purchaseText.text = $"{FormatString.Number(ArtefactItem.CostToUpgrade(BuyAmount))} (x{BuyAmount})";
            }

            upgradeButton.interactable = !ArtefactItem.IsMaxLevel && pp >= ArtefactItem.CostToUpgrade(BuyAmount);
        }


        // = = = Button Callbacks = = = //
        public void OnUpgradeArtefactBtn()
        {
            App.Data.Arts.UpgradeArtefact(_artefactId, BuyAmount, (success) => 
            {
                UpdateInterfacElements(); 
            });
        }
    }
}