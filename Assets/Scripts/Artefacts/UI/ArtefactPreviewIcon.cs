using SRC.Artefacts.Data;
using SRC.UI;
using UnityEngine;

namespace SRC.Artefacts.UI
{
    public class ArtefactPreviewIcon : SRC.Core.GMMonoBehaviour
    {
        [SerializeField] private GenericGradeItem GradeSlot;

        public void Intialize(AggregatedArtefactData artefact)
        {
            GradeSlot.Intialize(artefact);
        }
    }
}
