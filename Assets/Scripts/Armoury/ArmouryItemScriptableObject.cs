using UnityEngine;


namespace SRC.Armoury.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptables/ArmouryItemScriptableObject")]
    public class ArmouryItemScriptableObject : ScriptableObject
    {
        public int Id => int.Parse(name.Split('_')[0]);

        public Sprite Icon;
    }
}
