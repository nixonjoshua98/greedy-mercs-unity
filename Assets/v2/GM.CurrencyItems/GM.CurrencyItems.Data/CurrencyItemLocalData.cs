using UnityEngine;

namespace GM.CurrencyItems.Data
{
    [CreateAssetMenu(menuName = "Scriptables/CurrencyItemLocalData")]
    public class CurrencyItemLocalData : ScriptableObject
    {
        public CurrencyType Item;
        public Sprite Icon;

        public string DisplayName;
    }
}
