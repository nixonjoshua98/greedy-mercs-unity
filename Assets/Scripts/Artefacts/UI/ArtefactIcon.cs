using GM.Artefacts.Data;
using GM.Artefacts.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GM.Artefacts.UI
{
    public class ArtefactIcon : Core.GMMonoBehaviour
    {
        [Header("Optional")]
        public TMP_Text NameText;
        [Space]
        public Image IconImage;
        public Image BackgroundImage;

        public void Set(ArtefactGameDataModel artefact)
        {
            IconImage.sprite = artefact.Icon;

            if (NameText != null) NameText.text = artefact.Name;
            if (artefact.IconBackground != null) BackgroundImage.sprite = artefact.IconBackground;
        }

        public void Set(AggregatedArtefactData artefact)
        {
            Set(App.Artefacts.GetGameArtefact(artefact.Id));
        }
    }
}
