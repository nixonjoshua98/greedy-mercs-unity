using TMPro;
using UnityEngine;

namespace GM.UI
{
    public class CurrencyBanner : Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject ItemTextPopup;

        [Header("References")]
        public Transform PopupParent;
        [Space]
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
            App.Data.Inv.E_BountyPointsChange.AddListener((change) => { InstantiateQuantityPopup(BountyPointsText.transform.parent.position).Set(change); });
            App.Data.Inv.E_PrestigePointsChange.AddListener((change) => { InstantiateQuantityPopup(PrestigePointsText.transform.parent.position).Set(change); });
            App.Data.Inv.E_GoldChange.AddListener((change) => { InstantiateQuantityPopup(GoldText.transform.parent.position).Set(change); });
        }

        QuantityPopup InstantiateQuantityPopup(Vector3 pos)
        {
            return Instantiate<QuantityPopup>(ItemTextPopup, PopupParent, pos);
        }
    }
}
