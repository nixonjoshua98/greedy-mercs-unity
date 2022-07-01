using GM.Common.Enums;
using UnityEngine;

namespace GM.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptables/ItemGrade")]
    public class ItemGrade : ScriptableObject
    {
        public ItemGradeID Grade;

        [Header("Appearance")]
        public Sprite BackgroundSprite;
        public Color FrameColour;
    }
}
