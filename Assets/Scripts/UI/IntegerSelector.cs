using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GM.UI
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
        [Space]
        [SerializeField] List<QuantitySelectorOption> Options = new();

        int CurrentIndex = 0;
        QuantitySelectorOption Current => Options[CurrentIndex];

        public int CurrentValue => Current.Value;

        [Header("Events")]
        [HideInInspector] public UnityEvent<int> E_OnChange = new();

        void Awake()
        {
            UpdateButton();
        }

        void UpdateButton()
        {
            OptionButton.text = Current.Label;
        }

        /* Callbacks */

        public void OnOptionChange()
        {
            CurrentIndex = (CurrentIndex + 1) % Options.Count;

            UpdateButton();

            E_OnChange.Invoke(CurrentValue);
        }
    }
}
