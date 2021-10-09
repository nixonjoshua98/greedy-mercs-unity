using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Artefacts.UI
{
    public class ArtefactSlotDesign : MonoBehaviour
    {
        public Image Background;

        void Start()
        {
            int indexInParent = transform.GetSiblingIndex();

            if (indexInParent % 2 == 0)
            {
                SetAlternateDesign();
            }

            Destroy(this);
        }

        void SetAlternateDesign()
        {
            Background.color = new Color(32/255, 32/255, 32/255);
        }
    }
}
