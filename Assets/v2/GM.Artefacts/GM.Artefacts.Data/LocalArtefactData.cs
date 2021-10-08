using UnityEngine;


namespace GM.Artefacts.Data
{
    [CreateAssetMenu(menuName = "Scriptables/LocalArtefactData")]
    public class LocalArtefactData : ScriptableObject
    {
        // 0_Artefact or 1_SpecialOne or 2_Artefact_Name
        public int Id => int.Parse(name.Split('_')[0]);

        [Space]

        public string Name = "<Missing Artefact Name>";

        public Sprite Icon;
        public GM.Artefacts.UI.ArtefactSlot Slot;
    }
}