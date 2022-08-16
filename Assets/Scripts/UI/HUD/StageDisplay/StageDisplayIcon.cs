using TMPro;
using UnityEngine;

namespace SRC.UI.HUD.StageDisplay
{
    public class StageDisplayIcon : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] TMP_Text StageText;

        public void UpdateIcon(string text)
        {
            StageText.text = text;
        }
    }
}
