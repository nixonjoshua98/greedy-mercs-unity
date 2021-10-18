using UnityEngine;


namespace GM.Armoury.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptables/LocalArmouryItemData")]
    public class LocalArmouryItemData : ScriptableObject
    {
        public int Id => int.Parse(name.Split('_')[0]);

        [Space]

        public string Name = "<Missing Name>";

        public Sprite Icon;
    }
}
