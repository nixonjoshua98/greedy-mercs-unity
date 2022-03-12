using TMPro;
using UnityEngine;
using GM.Common;
using BigInteger = System.Numerics.BigInteger;

namespace GM.UI
{
    public class CurrencyBanner : Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject ItemTextPopup;

        [Header("References")]
        [SerializeField] ObjectPool TextPool;
        public Transform PopupParent;
        [Space]
        public TMP_Text GoldText;
        public TMP_Text BountyPointsText;
        public TMP_Text ArmouryPointsText;
        public TMP_Text PrestigePointsText;

        void FixedUpdate()
        {
            GoldText.text = Format.Number(App.Inventory.Gold);
            BountyPointsText.text = Format.Number(App.Inventory.BountyPoints);
            ArmouryPointsText.text = Format.Number(App.Inventory.ArmouryPoints);
            PrestigePointsText.text = Format.Number(App.Inventory.PrestigePoints);
        }

        void Start()
        {
            App.Events.BountyPointsChanged.AddListener((change) => { ShowText(BountyPointsText, change); });
            App.Events.PrestigePointsChanged.AddListener((change) => { ShowText(PrestigePointsText, change); });
            App.Events.GoldChanged.AddListener((change) => { ShowText(GoldText, change); });
            App.Events.ArmouryPointsChanged.AddListener((change) => { ShowText(ArmouryPointsText, change); });
        }

        void ShowText(TMP_Text txt, BigInteger value)
        {
            TextPopup o = TextPool.Spawn<TextPopup>();

            o.transform.position = txt.transform.parent.position;

            o.Set(value);
        }

        void ShowText(TMP_Text txt, BigDouble value)
        {
            TextPopup o = TextPool.Spawn<TextPopup>();

            o.transform.position = txt.transform.parent.position;

            o.Set(value);
        }
    }
}
