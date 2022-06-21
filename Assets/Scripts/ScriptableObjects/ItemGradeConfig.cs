using GM.Common.Enums;
using UnityEngine;

namespace GM.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptables/ItemGradeConfig")]
    public class ItemGradeConfig : ScriptableObject
    {
        public ItemGrade Grade;

        [Header("Appearance")]
        public Sprite BackgroundSprite;
        public Color FrameColour;
    }
}
