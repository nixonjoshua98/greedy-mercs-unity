using UnityEngine;

namespace GM.CurrencyItems.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptables/CurrencyItemScriptableObject")]
    public class CurrencyItemScriptableObject : ScriptableObject
    {
        public Common.Enums.CurrencyType Item;
        public Sprite Icon;

        public string DisplayName;
    }
}
