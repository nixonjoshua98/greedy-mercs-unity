using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SRC.UI
{
    [System.Serializable]
    internal struct QuantitySelectorOption
    {
        public string Label;
        public int Value;
    }

    public class IntegerSelector : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text OptionButton;

        [Header("Initialization")]
        [SerializeField] private int _IntialIndex = 0;
        [SerializeField] private List<QuantitySelectorOption> Options = new();

        [Header("Events")]
        [HideInInspector] public UnityEvent<int> E_OnChange = new();
        private int CurrentIndex;

        private QuantitySelectorOption Current => Options[CurrentIndex];
        public int CurrentValue => Current.Value;

        private void Awake()
        {
            CurrentIndex = _IntialIndex;

            UpdateButton();
        }

        private void UpdateButton()
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
