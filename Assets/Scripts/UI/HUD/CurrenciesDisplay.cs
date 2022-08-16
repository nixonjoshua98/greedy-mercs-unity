using SRC.Common;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace SRC.UI.HUD
{
    struct PropertyTracker<T> where T : IComparable<T>
    {
        public T StartValue;
        public Func<T> GetCurrentValue;

        public PropertyTracker(T startValue, Func<T> currentValue)
        {
            StartValue = startValue;
            GetCurrentValue = currentValue;
        }
        public void Reset()
        {
            StartValue = GetCurrentValue();
        }

        public bool HasChanged => GetCurrentValue().CompareTo(StartValue) != 0;
    }

    public class CurrenciesDisplay : SRC.Core.GMMonoBehaviour
    {
        [Header("Text Elements")]
        [SerializeField] TMP_Text LargeGoldText;
        [SerializeField] TMP_Text SmallGoldText;

        [Header("Prefabs")]
        [SerializeField] GameObject GoldTravelPS;

        [Header("Transforms")]
        [SerializeField] Transform GoldIconTransform;

        [Header("References")]
        [SerializeField] ObjectPool TextNumbers;

        float _lastDisplayUpdate;

        int _numGoldTrails = 0;

        PropertyTracker<BigDouble> GoldTracker;

        void Awake()
        {
            GoldTracker = new(App.Inventory.Gold, () => App.Inventory.Gold);

            UpdateDisplayGold();
        }

        void Start()
        {
            InvokeRepeating(nameof(UpdateDisplayGoldTask), 0.0f, 0.1f);
        }

        public void DisplayGoldTrail(Vector3 startPos)
        {
            StartCoroutine(DisplayGoldTrailEnumerator(startPos));
        }

        void PulseGoldIcon()
        {
            StartCoroutine(PulseGoldIconEnumerator());
        }

        void UpdateDisplayGoldTask()
        {
            // Force a UI update at least 4 times a second
            if (GoldTracker.HasChanged && _numGoldTrails == 0 && Mathf.Abs(Time.timeSinceLevelLoad - _lastDisplayUpdate) > 0.25f)
            {
                UpdateDisplayGold();
            }
        }

        void UpdateDisplayGold()
        {
            _lastDisplayUpdate = Time.timeSinceLevelLoad;

            LargeGoldText.text = Format.Number(App.Inventory.Gold, decimalPlaces: 1);
            SmallGoldText.text = LargeGoldText.text;

            BigDouble changeSinceUpdate = GoldTracker.GetCurrentValue() - GoldTracker.StartValue;

            GoldTracker.Reset();

            if (changeSinceUpdate != 0)
                ShowSmallText(SmallGoldText.transform.position, changeSinceUpdate);
        }

        IEnumerator DisplayGoldTrailEnumerator(Vector3 startPos)
        {
            _numGoldTrails++;

            GameObject ps = Instantiate(GoldTravelPS, startPos, Quaternion.identity);

            // Lerp the particle system position
            yield return this.Lerp(0, 1, 0.35f, (progress) =>
            {
                Vector3 endPos = Camera.main.ScreenToWorldPoint(GoldIconTransform.transform.position);

                ps.transform.position = Vector2.Lerp(startPos, endPos, progress);
            });

            PulseGoldIcon();

            Destroy(ps, 0.25f); // TODO: Replace with an object pool

            _numGoldTrails--;
        }

        IEnumerator PulseGoldIconEnumerator()
        {
            // 'Flip' the display gold during the pulse animation
            Invoke(nameof(UpdateDisplayGold), 0.1f);

            // 'Pulse' the gold icon
            yield return this.Lerp(0, 1, 0.15f, (progress) =>
            {
                float scale = Mathf.Lerp(1, 1.25f, progress);

                GoldIconTransform.localScale = new(scale, scale, 0);
            });

            // Reset the scale to the original value
            GoldIconTransform.localScale = Vector3.one;
        }

        private void ShowSmallText(Vector3 position, BigDouble value)
        {
            TextPopup popup = TextNumbers.Spawn<TextPopup>();

            popup.transform.position = position;

            popup.Set(value);
        }
    }
}
