using GMCommon.Enums;
using UnityEngine;

namespace GM.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptables/ItemGradeConfig")]
    public class ItemGradeConfig : ScriptableObject
    {
        public ItemGrade Grade;
        public string Name;

        [Header("Appearance")]
        public Sprite BackgroundSprite;
        public Color FrameColour;
    }
}
