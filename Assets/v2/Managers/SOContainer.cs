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

        public static ItemGradeConfig GetItemGradeConfig(ItemGrade grade) => _instance._itemGradesConfig.FirstOrDefault(x => x.Grade == grade);

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
