using UnityEngine;

namespace GM.Artefacts.Scriptables
{
    [CreateAssetMenu(menuName = "Scriptables/ArtefactScriptableObject")]
    public class ArtefactScriptableObject : ScriptableObject
    {
        // 0_Artefact or 1_SpecialOne or 2_Artefact_Name
        public int Id => int.Parse(name.Split('_')[0]);

        public Sprite Icon;
    }
}