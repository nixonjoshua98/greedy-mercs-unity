using GM.Common.Enums;
using GMCommon.Enums;
using System.Collections.Generic;
using System.Linq;

namespace GM.ScriptableObjects
{
    public class LocalGameDataContainer
    {
        public List<ItemGradeConfig> ItemGradesConfigs;
        public List<CurrencyConfig> CurrencyItemsConfig;

        public ItemGradeConfig GetItemGradeConfig(ItemGrade grade) => ItemGradesConfigs.FirstOrDefault(x => x.Grade == grade);
        public CurrencyConfig GetCurrencyConfig(CurrencyType type) => CurrencyItemsConfig.FirstOrDefault(x => x.Type == type);
    }
}