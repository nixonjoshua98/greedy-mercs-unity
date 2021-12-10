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
        public GM.UI.DestroyButton DestroyButton;
        public Button UnlockButton;
        public TMP_Text PlaceholderText;
        public Image ArtefactIconImage;
        public TMP_Text ArtefactText;
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

            ArtefactIconImage.enabled = true; // Enable the image so we can actually see it

            // Disable some elements while the animation is running
            DestroyButton.interactable = UnlockButton.interactable = PlaceholderText.enabled = false;

            List<ArtefactGameDataModel> artefacts = App.Data.Artefacts.GameArtefactsList;

            while (animationTimer > 0.0f)
            {
                // Generate a random frame time
                float frameTime = UnityEngine.Random.Range(0.1f, 0.15f);

                // Random artefact
                ArtefactGameDataModel artefact = artefacts[UnityEngine.Random.Range(0, artefacts.Count - 1)];

                // Update some UI
                UpdateArtefactUI(artefact.Name, artefact.Icon);

                // Patse for a period of time
                yield return new WaitForSecondsRealtime(frameTime);

                // Reduce the overall timer
                animationTimer -= frameTime;
            }

            UpdateArtefactUI(unlockedArtefact.Name, unlockedArtefact.Icon);

            // Re-enable buttons
            DestroyButton.interactable = UnlockButton.interactable = true;

            finishedAction.Invoke();
        }

        void UpdateUnlockButtonText()
        {
            UnlockButton.interactable = true;

            BigInteger unlockCost = App.Cache.ArtefactUnlockCost(App.Data.Artefacts.NumUnlockedArtefacts);

            if (unlockCost > App.Data.Inv.PrestigePoints)
            {
                UnlockButton.interactable = false;
                UnlockButtonText.text = "Cannot Afford";
            }
            else if (App.Data.Artefacts.UserUnlockedAll)
            {
                UnlockButton.interactable = false;
                UnlockButtonText.text = "Unlocked all";
            }
            else
            {
                UnlockButtonText.text = "Unlock Artefact";
            }
        }

        void UpdateArtefactUI(string name, Sprite icon)
        {
            ArtefactText.text = name;
            ArtefactIconImage.sprite = icon;
        }

        public void OnUnlockButton()
        {
            App.Data.Artefacts.UnlockArtefact((success, artefact) =>
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
