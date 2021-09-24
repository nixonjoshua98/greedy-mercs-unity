using UnityEngine;

namespace GM.Items.Data
{
    [CreateAssetMenu(menuName = "Scriptables/LocalItemData")]
    public class LocalItemData : ScriptableObject
    {
        public ItemType Item;
        public Sprite Icon;

        public string DisplayName;
    }
}
