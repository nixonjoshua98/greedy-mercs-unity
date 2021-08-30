
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Artefacts
{
    using GM.UI;

    public class ArtefactSlot : MonoBehaviour
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

        ArtefactData artefactData => GameData.Get.Artefacts.Get(_artefactId);
        ArtefactState artefactState => UserData.Get.Artefacts.Get(_artefactId);

        int BuyAmount => MathUtils.NextMultipleMax(artefactState.Level, _buyAmount, artefactData.MaxLevel);

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
            nameText.text = artefactData.Name;

            icon.sprite = artefactData.Icon;
        }


        void UpdateInterfacElements()
        {
            if (!_updatingUi)
                return;
            
            BigInteger pp = UserData.Get.Inventory.PrestigePoints;

            levelText.text  = $"Lvl. {artefactState.Level}";
            effectText.text = FormatString.Bonus(artefactData.Bonus, artefactState.Effect());

            purchaseText.text = "-";

            if (!artefactState.IsMaxLevel())
            {
                purchaseText.text = $"{FormatString.Number(artefactState.CostToUpgrade(BuyAmount))} (x{BuyAmount})";
            }

            upgradeButton.interactable = !artefactState.IsMaxLevel() && pp >= artefactState.CostToUpgrade(BuyAmount);
        }


        // = = = Button Callbacks = = = //
        public void OnUpgradeArtefactBtn()
        {
            UserData.Get.Artefacts.UpgradeArtefact(_artefactId, BuyAmount, (_) => 
            {
                UpdateInterfacElements(); 
            });
        }
    }
}