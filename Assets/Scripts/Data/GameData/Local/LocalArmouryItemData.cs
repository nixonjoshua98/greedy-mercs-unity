
using UnityEngine;


namespace GM.Data
{
    [CreateAssetMenu(menuName = "Scriptables/LocalArmouryItemData")]
    public class LocalArmouryItemData : ScriptableObject
    {
        public int ID;

        [Space]

        public string Name = "<Missing Name>";

        public Sprite Icon;
    }
}
