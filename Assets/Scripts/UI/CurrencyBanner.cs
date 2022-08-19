using SRC.Common;
using TMPro;
using UnityEngine;

namespace SRC.UI
{
    public class CurrencyBanner : Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject ItemTextPopup;

        [Header("References")]
        [SerializeField] private ObjectPool TextPool;
        public Transform PopupParent;
        [Space]
        public TMP_Text BountyPointsText;
        public TMP_Text ArmouryPointsText;
        public TMP_Text GemstoneText;
        public TMP_Text PrestigePointsText;

        private void FixedUpdate()
        {
            BountyPointsText.text = Format.Number(App.Inventory.BountyPoints);
            ArmouryPointsText.text = Format.Number(App.Inventory.ArmouryPoints);
            PrestigePointsText.text = Format.Number(App.Inventory.PrestigePoints);
            GemstoneText.text = Format.Number(App.Inventory.Gemstones);
        }

        private void Start()
        {
            App.Inventory.BountyPointsChanged.AddListener((change) => { ShowText(BountyPointsText, change); });
            App.Inventory.PrestigePointsChanged.AddListener((change) => { ShowText(PrestigePointsText, change); });
            App.Inventory.ArmouryPointsChanged.AddListener((change) => { ShowText(ArmouryPointsText, change); });
        }

        private void ShowText(TMP_Text txt, BigDouble value)
        {
            GetTextPopup(txt).Set(value);
        }

        private void ShowText(TMP_Text txt, long value)
        {
            GetTextPopup(txt).Set(value);
        }

        private TextPopup GetTextPopup(TMP_Text text)
        {
            TextPopup o = TextPool.Spawn<TextPopup>();

            o.transform.position = text.transform.parent.position;

            return o;
        }
    }
}
