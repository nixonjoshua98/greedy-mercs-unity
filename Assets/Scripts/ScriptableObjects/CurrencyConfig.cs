using GM.Common.Enums;
using UnityEngine;

namespace GM.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptables/CurrencyConfig")]
    public class CurrencyConfig : ScriptableObject
    {
        public CurrencyType Type;
        public Sprite Icon;
        public string DisplayName;
    }
}
