using SRC.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SRC.Inventory.Scriptables
{
    [System.Serializable]
    public class Currency
    {
        public string DisplayName;
        public CurrencyType Type;
        public Sprite Icon;
        public Color Colour = Color.white;

        [TextArea]
        public string ShortDescription;
    }

    [CreateAssetMenu(menuName = "Scriptables/CurrencyLocalDataFile")]
    public class CurrencyLocalDataFile : ScriptableObject
    {
        public List<Currency> Currencies = new();

        public Currency Get(CurrencyType currencyType) => Currencies.FirstOrDefault(x => x.Type == currencyType);
    }
}
