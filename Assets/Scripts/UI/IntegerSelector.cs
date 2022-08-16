using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SRC.UI
{
    [System.Serializable]
    struct QuantitySelectorOption
    {
        public string Label;
        public int Value;
    }

    public class IntegerSelector : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] TMP_Text OptionButton;

        [Header("Initialization")]
        [SerializeField] int _IntialIndex = 0;
        [SerializeField] List<QuantitySelectorOption> Options = new();

        [Header("Events")]
        [HideInInspector] public UnityEvent<int> E_OnChange = new();

        int CurrentIndex;
        QuantitySelectorOption Current => Options[CurrentIndex];
        public int CurrentValue => Current.Value;

        void Awake()
        {
            CurrentIndex = _IntialIndex;

            UpdateButton();
        }

        void UpdateButton()
        {
            OptionButton.text = Current.Label;
        }

        public void InvokeChange()
        {
            E_OnChange?.Invoke(CurrentValue);
        }

        /* Callbacks */

        public void OnOptionChange()
        {
            CurrentIndex = (CurrentIndex + 1) % Options.Count;

            UpdateButton();
            InvokeChange();
        }
    }
}
