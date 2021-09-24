
using UnityEngine;


namespace GM.Armoury.Data
{
    [CreateAssetMenu(menuName = "Scriptables/LocalArmouryItemData")]
    public class LocalArmouryItemData : ScriptableObject
    {
        // 0_Artefact or 1_SpecialOne or 2_Artefact_Name
        public int ID => int.Parse(name.Split('_')[0]);

        [Space]

        public string Name = "<Missing Name>";

        public Sprite Icon;
    }
}
