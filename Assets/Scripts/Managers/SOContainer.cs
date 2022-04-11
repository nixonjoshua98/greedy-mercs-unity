using GM.Common.Enums;
using GMCommon.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GM.ScriptableObjects
{
    public class SOContainer : MonoBehaviour
    {
        static SOContainer _instance = null;

        [SerializeField] List<ItemGradeConfig> _itemGradesConfig;
        [SerializeField] List<CurrencyConfig> _currencyItemsConfig;

        public static ItemGradeConfig GetItemGradeConfig(ItemGrade grade) => _instance._itemGradesConfig.FirstOrDefault(x => x.Grade == grade);
        public static CurrencyConfig GetCurrencyConfig(CurrencyType type) => _instance._currencyItemsConfig.FirstOrDefault(x => x.Type == type);


        public void Awake()
        {
            if (_instance is null)
            {
                _instance = this;

                DontDestroyOnLoad(this);
            }
            else
            {
                GMLogger.Editor("Destroyed duplicate 'ScriptableObjectManager' found");
                Destroy(this);
            }
        }
    }
}
