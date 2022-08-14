using TMPro;
using UnityEngine;

namespace GM.UI.HUD.StageDisplay
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
