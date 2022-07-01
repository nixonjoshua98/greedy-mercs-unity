using GM.Common.Enums;
using System.Collections.Generic;
using System.Linq;

namespace GM.ScriptableObjects
{
    [System.Serializable]
    public class LocalDataContainer
    {
        public List<ItemGradeData> ItemGrades;
        public List<CurrencyConfig> Currencies;

        public ItemGradeData GetItemGrade(ItemGradeID grade) => ItemGrades.FirstOrDefault(x => x.Grade == grade);
        public CurrencyConfig GetCurrency(CurrencyType type) => Currencies.FirstOrDefault(x => x.Type == type);
    }
}