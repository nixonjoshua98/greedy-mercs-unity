using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GM.UI_
{
    enum OptionsViewState { SHOWN = 0, HIDDEN = 1 }

    public class AmountSelector : MonoBehaviour
    {
        [Header("Select Button References")]
        public Button SelectButton;
        public TMP_Text SelectButtonText;

        [Header("Options")]
        public int DefaultOptionIndex;
        [Space]
        public Button[] Options;
        public int[] OptionValues;

        OptionsViewState optionsState;

        [HideInInspector] public UnityEvent<int> E_OnChange = new UnityEvent<int>();
        [HideInInspector] public int Current;

        void Awake()
        {
            optionsState = OptionsViewState.HIDDEN;

            HideOptions();
            AssignOptionCallbacks();
            SetDefaultValues();
        }

        void AssignOptionCallbacks()
        {
            for (int i = 0; i < Options.Length; ++i)
            {
                Button t = Options[i];

                AssignCallback(t, OptionValues[i]);
            }
        }

        void AssignCallback(Button t, int value) => t.onClick.AddListener(() => { OnOptionButtonDown(t, value); });

        void SetDefaultValues()
        {
            Button btn = Options[DefaultOptionIndex];
            int value = OptionValues[DefaultOptionIndex];

            TMP_Text txt = btn.GetComponentInChildren<TMP_Text>();

            Current = value;
            SelectButtonText.text = txt.text;

            HideOptions();

            E_OnChange.Invoke(value);
        }

        // == Callbacks == //
        public void OnSelectButton()
        {
            if (optionsState == OptionsViewState.HIDDEN)
            {
                ShowOptions();
            }
        }

        void OnOptionButtonDown(Button btn, int value)
        {
            TMP_Text txt = btn.GetComponentInChildren<TMP_Text>();

            SelectButtonText.text = txt.text;

            HideOptions();

            E_OnChange.Invoke(value);
        }


        // == Animations == //
        void HideOptions()
        {
            SelectButton.gameObject.SetActive(true);

            foreach (Button t in Options)
            {
                t.gameObject.SetActive(false);
            }

            optionsState = OptionsViewState.HIDDEN;
        }

        void ShowOptions()
        {
            SelectButton.gameObject.SetActive(false);

            foreach (Button t in Options)
            {
                t.gameObject.SetActive(true);
            }

            optionsState = OptionsViewState.SHOWN;
        }
    }
}