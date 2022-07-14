using GM.Enums;
using UnityEngine;

namespace GM.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptables/ItemGradeData")]
    public class ItemGradeData : ScriptableObject
    {
        public ItemGrade Grade;

        [Header("Appearance")]
        public Sprite BackgroundSprite;
        public Color FrameColour;
    }
}
