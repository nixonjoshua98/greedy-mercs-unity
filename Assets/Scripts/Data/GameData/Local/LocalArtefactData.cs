using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GM.Artefacts
{
    [CreateAssetMenu(menuName = "Scriptables/LocalArtefactData")]
    public class LocalArtefactData : ScriptableObject
    {
        public int ID;

        [Space]

        public string Name = "<Missing Artefact Name>";

        public Sprite Icon;
    }
}
