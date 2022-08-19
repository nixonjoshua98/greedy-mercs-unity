using SRC.Common.Enums;
using SRC.Inventory.Scriptables;
using System.Collections.Generic;
using System.Linq;

namespace SRC.ScriptableObjects
{
    [System.Serializable]
    public class LocalDataContainer
    {
        public List<ItemGradeData> ItemGrades;
        public CurrencyLocalDataFile Currencies;

        public ItemGradeData GetItemGrade(Rarity grade) => ItemGrades.FirstOrDefault(x => x.Grade == grade);
    }
}