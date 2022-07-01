using GM.Common.Enums;
using System.Collections.Generic;
using System.Linq;

namespace GM.ScriptableObjects
{
    [System.Serializable]
    public class LocalDataContainer
    {
        public List<ItemGrade> ItemGradesConfigs;
        public List<CurrencyConfig> CurrencyItemsConfig;

        public ItemGrade GetItemGradeConfig(ItemGradeID grade) => ItemGradesConfigs.FirstOrDefault(x => x.Grade == grade);
        public CurrencyConfig GetCurrencyConfig(CurrencyType type) => CurrencyItemsConfig.FirstOrDefault(x => x.Type == type);
    }
}