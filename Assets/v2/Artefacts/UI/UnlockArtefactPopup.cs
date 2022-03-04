using GM.Artefacts.Data;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using System.Numerics;
using GM.Artefacts.Models;
using System.Collections.Generic;
using UnityEngine.UI;

namespace GM.Artefacts.UI
{
    public class UnlockArtefactPopup : Core.GMMonoBehaviour
    {
        [Header("References")]
        public ArtefactIcon Icon;

        public GM.UI.DestroyButton DestroyButton;
        public Button UnlockButton;
        public TMP_Text UnlockButtonText;

        Action<ArtefactData> OnArtefactUnlocked;

        public void Init(Action<ArtefactData> action)
        {
            OnArtefactUnlocked = action;
        }

        IEnumerator ArtefactRotationAnimation(ArtefactData unlockedArtefact, Action finishedAction)
        {
            // Fetch a random animation duration
            float animationTimer = UnityEngine.Random.Range(2.5f, 4.0f);

            Icon.IconImage.enabled = true; // Enable the image so we can actually see it

            // Disable some elements while the animation is running
            DestroyButton.interactable = UnlockButton.interactable = false;

            List<ArtefactGameDataModel> artefacts = App.GMData.Artefacts.GameArtefactsList;

            while (animationTimer > 0.0f)
            {
                // Generate a random frame time
                float frameTime = UnityEngine.Random.Range(0.1f, 0.15f);

                // Random artefact
                ArtefactGameDataModel artefact = artefacts[UnityEngine.Random.Range(0, artefacts.Count - 1)];

                Icon.Set(artefact);

                // Patse for a period of time
                yield return new WaitForSecondsRealtime(frameTime);

                // Reduce the overall timer
                animationTimer -= frameTime;
            }

            Icon.Set(unlockedArtefact);

            // Re-enable buttons
            DestroyButton.interactable = UnlockButton.interactable = true;

            finishedAction.Invoke();
        }

        void UpdateUnlockButtonText()
        {
            UnlockButton.interactable = true;

            BigInteger unlockCost = App.GMCache.ArtefactUnlockCost(App.GMData.Artefacts.NumUnlockedArtefacts);

            if (unlockCost > App.GMData.Inv.PrestigePoints)
            {
                UnlockButton.interactable = false;
                UnlockButtonText.text = "Cannot Afford";
            }
            else if (App.GMData.Artefacts.UserUnlockedAll)
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
            App.GMData.Artefacts.UnlockArtefact((success, artefact) =>
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