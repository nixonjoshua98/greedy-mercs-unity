using SRC.Artefacts.Data;
using System.Collections;
using TMPro;
using UnityEngine;

namespace SRC.Artefacts.UI
{
    public class ArtefactsPreviewPanel : SRC.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject ArtefactPreviewIconObject;

        [Header("Text Elements")]
        [SerializeField] private TMP_Text CollectedText;

        [Header("References")]
        [SerializeField] private Transform PreviewIconsParent;

        private IEnumerator Start()
        {
            CollectedText.text = $"Artefacts {App.Artefacts.NumUnlockedArtefacts} / {App.Artefacts.MaxArtefacts}";

            foreach (var artefact in App.Artefacts.Artefacts)
            {
                yield return new WaitForFixedUpdate();

                InstantiatePreviewIcon(artefact);
            }
        }

        private void InstantiatePreviewIcon(AggregatedArtefactData artefact)
        {
            this.Instantiate<ArtefactPreviewIcon>(ArtefactPreviewIconObject, PreviewIconsParent).Intialize(artefact);
        }

        public void ClosePanel()
        {
            Destroy(gameObject);
        }
    }
}
