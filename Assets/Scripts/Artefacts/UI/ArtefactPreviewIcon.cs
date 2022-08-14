using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GM.Artefacts.Data;
using GM.Artefacts.Data;
using TMPro;
using System.Threading.Tasks;
using GM.UI;

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
