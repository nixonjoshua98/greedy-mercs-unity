using GMCommon.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GM.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptables/ItemGradeConfig")]
    public class ItemGradeConfig : ScriptableObject
    {
        public ItemGrade Grade;
        public string Name;

        [Header("Appearance")]
        public Sprite BackgroundSprite;
    }
}
