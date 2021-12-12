using UnityEngine;
using GM.Artefacts.Data;

namespace GM.Artefacts.Scriptables
{
    [CreateAssetMenu(menuName = "Scriptables/ArtefactScriptableObject")]
    public class ArtefactScriptableObject : ScriptableObject
    {
        // 0_Artefact or 1_SpecialOne or 2_Artefact_Name
        public int Id => int.Parse(name.Split('_')[0]);

        [Space]

        public string Name = "<Missing Artefact Name>";

        public Sprite Icon;
        public Sprite IconBackground;
    }
}