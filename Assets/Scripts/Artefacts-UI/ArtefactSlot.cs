﻿
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Artefacts
{
    using GM.Data;
    using GM.UI;

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

        ArtefactData artGameData { get { return GameData.Get().Artefacts.Get(_artefactId); } }

        int BuyAmount
        { 
            get 
            {
                ArtefactState artState  = ArtefactManager.Instance.Get(_artefactId);

                return MathUtils.NextMultipleMax(artState.Level, _buyAmount, artGameData.MaxLevel);
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

            SetInterfaceElements();

            UpdateInterfacElements();
        }


        void SetInterfaceElements()
        {
            nameText.text = artGameData.Name;

            icon.sprite = artGameData.Icon;
        }


        void UpdateInterfacElements()
        {
            if (!_updatingUi)
                return;

            ArtefactData artData = GameData.Get().Artefacts.Get(_artefactId);
            ArtefactState artState = ArtefactManager.Instance.Get(_artefactId);

            BigInteger pp = UserData.Get().Inventory.PrestigePoints;

            levelText.text  = $"Lvl. {artState.Level}";
            effectText.text = FormatString.Bonus(artData.Bonus, artState.Effect());

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