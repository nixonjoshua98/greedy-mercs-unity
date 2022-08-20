using SRC.Artefacts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SRC.Artefacts.UI
{
    public class UnlockArtefactPopup : Core.GMMonoBehaviour
    {
        //[Header("References")]
        //public ArtefactIcon Icon;

        public SRC.UI.DestroyButton DestroyButton;
        public Button UnlockButton;
        public TMP_Text UnlockButtonText;
        private Action<AggregatedArtefactData> OnArtefactUnlocked;

        public void Init(Action<AggregatedArtefactData> action)
        {
            OnArtefactUnlocked = action;
        }

        private IEnumerator ArtefactRotationAnimation(AggregatedArtefactData unlockedArtefact, Action finishedAction)
        {
            // Fetch a random animation duration
            float animationTimer = UnityEngine.Random.Range(2.5f, 4.0f);

            //Icon.IconImage.enabled = true; // Enable the image so we can actually see it

            // Disable some elements while the animation is running
            DestroyButton.interactable = UnlockButton.interactable = false;

            List<Artefact> artefacts = App.Artefacts.GameArtefactsList;

            while (false && animationTimer > 0.0f)
            {
                // Generate a random frame time
                float frameTime = UnityEngine.Random.Range(0.1f, 0.15f);

                // Random artefact
                Artefact artefact = artefacts[UnityEngine.Random.Range(0, artefacts.Count - 1)];

                //Icon.Set(artefact);

                // Patse for a period of time
                yield return new WaitForSecondsRealtime(frameTime);

                // Reduce the overall timer
                animationTimer -= frameTime;
            }

            //Icon.Set(unlockedArtefact);

            // Re-enable buttons
            DestroyButton.interactable = UnlockButton.interactable = true;

            finishedAction.Invoke();
        }

        private void UpdateUnlockButtonText()
        {
            UnlockButton.interactable = true;

            var unlockCost = App.Bonuses.ArtefactUnlockCost(App.Artefacts.NumUnlockedArtefacts);

            if (unlockCost > App.Inventory.PrestigePoints)
            {
                UnlockButton.interactable = false;
                UnlockButtonText.text = "Cannot Afford";
            }
            else if (App.Artefacts.UserUnlockedAll)
            {
                UnlockButton.interactable = false;
                UnlockButtonText.text = "Unlocked all";
            }
            else
            {
                UnlockButtonText.text = "Unlock Artefact";
            }
        }

        public void OnUnlockButton()
        {
            App.Artefacts.UnlockArtefact((success, artefact) =>
            {
                if (success)
                {
                    var coro = ArtefactRotationAnimation(artefact, () =>
                    {
                        UpdateUnlockButtonText();

                        OnArtefactUnlocked.Invoke(artefact);
                    });

                    StartCoroutine(coro);
                }
            });
        }
    }
}