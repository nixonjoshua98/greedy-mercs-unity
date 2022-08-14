using GM.Artefacts.Data;
using GM.UI;
using UnityEngine;

namespace GM.Artefacts.UI
{
    public class ArtefactPreviewIcon : GM.Core.GMMonoBehaviour
    {
        [SerializeField] GenericGradeItem GradeSlot;

        public void Intialize(AggregatedArtefactData artefact)
        {
            GradeSlot.Intialize(artefact);
        }
    }
}
