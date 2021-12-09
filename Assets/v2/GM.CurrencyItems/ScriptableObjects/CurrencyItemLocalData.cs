using UnityEngine;

namespace GM.CurrencyItems.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptables/CurrencyItemLocalData")]
    public class CurrencyItemLocalData : ScriptableObject
    {
        public Common.Enums.CurrencyType Item;
        public Sprite Icon;

        public string DisplayName;
    }
}
