using SRC.Common.Enums;
using UnityEngine;

namespace SRC.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptables/CurrencyConfig")]
    public class CurrencyConfig : ScriptableObject
    {
        public CurrencyType Type;
        public Sprite Icon;
        public Color Colour = Color.white;
        public string DisplayName;
    }
}
