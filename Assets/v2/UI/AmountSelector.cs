using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GM.UI
{
    enum OptionsViewState { SHOWN, HIDDEN }

    public class AmountSelector : MonoBehaviour
    {
        [Header("References")]
        public Button SelectButton;
        public TMP_Text SelectButtonText;
        public HorizontalLayoutGroup ButtonsGroup;
        [Space]
        public List<Button> Options;
        public int[] OptionValues;

        OptionsViewState optionsState = OptionsViewState.HIDDEN;

        [HideInInspector] public UnityEvent<int> E_OnChange = new UnityEvent<int>();

        int currentButtonIndex = 0; // Current index for button selected

        public int Current => OptionValues[currentButtonIndex];

        void Awake()
        {
            HideOptions();
            AssignOptionCallbacks();
        }

        /// <summary>
        /// Set some default values after creation
        /// </summary>
        void Start()
        {
            Button btn = Options[0];

            TMP_Text txt = btn.GetComponentInChildren<TMP_Text>();

            SelectButtonText.text = txt.text;

            HideOptions();
        }

        public void InvokeChangeEvent()
        {
            E_OnChange.Invoke(Current);
        }


        /// <summary>
        /// Assign all callbacks to each button
        /// </summary>
        void AssignOptionCallbacks()
        {
            for (int i = 0; i < Options.Count; ++i)
            {
                Button t = Options[i];

                AssignCallback(t, i);
            }
        }

        /// <summary>
        /// Assign the callback to the button and remove all other listeners
        /// </summary>
        void AssignCallback(Button t, int index)
        {
            t.onClick.AddListener(() => { OnOptionButtonDown(t, index); });
        }

        /// <summary>
        /// Button callback for when slector is first clicked
        /// </summary>
        public void OnSelectButton()
        {
            if (optionsState == OptionsViewState.HIDDEN)
            {
                ShowOptions();
            }
        }

        /// <summary>
        /// Callback for when an option button is clicked (e.g 1x, 10x, 100x...)
        /// </summary>
        void OnOptionButtonDown(Button btn, int index)
        {
            TMP_Text txt = btn.GetComponentInChildren<TMP_Text>();

            SelectButtonText.text = txt.text;

            HideOptions();

            currentButtonIndex = index;

            E_OnChange.Invoke(Current);
        }

        void HideOptions()
        {
            SelectButton.gameObject.SetActive(true);

            Options.ForEach(opt => opt.gameObject.SetActive(false));

            optionsState = OptionsViewState.HIDDEN;
        }

        void ShowOptions()
        {
            SelectButton.gameObject.SetActive(false);

            Options.ForEach(opt => opt.gameObject.SetActive(true));

            optionsState = OptionsViewState.SHOWN;
        }
    }
}