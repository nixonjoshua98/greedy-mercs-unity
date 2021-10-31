using TMPro;
using UnityEngine;

namespace GM.UI
{
    public class CurrencyBanner : Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject ItemTextPopup;

        [Header("References")]
        public TMP_Text GoldText;
        public TMP_Text BountyPointsText;
        public TMP_Text ArmouryPointsText;
        public TMP_Text PrestigePointsText;

        void FixedUpdate()
        {
            GoldText.text = Format.Number(App.Data.Inv.Gold);
            BountyPointsText.text = Format.Number(App.Data.Inv.BountyPoints);
            ArmouryPointsText.text = Format.Number(App.Data.Inv.ArmouryPoints);
            PrestigePointsText.text = Format.Number(App.Data.Inv.PrestigePoints);
        }

        void Start()
        {
            App.Data.Inv.E_BountyPointsChange.AddListener((change) => { Instantiate<QuantityPopup>(ItemTextPopup, BountyPointsText.transform.parent.position).Set(change); });
            App.Data.Inv.E_PrestigePointsChange.AddListener((change) => { Instantiate<QuantityPopup>(ItemTextPopup, PrestigePointsText.transform.parent.position).Set(change); });
        }
    }
}
