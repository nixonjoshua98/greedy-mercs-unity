using System.Collections;
using TMPro;
using UnityEngine;

namespace GM.UI.HUD
{
    public class CurrenciesDisplay : GM.Core.GMMonoBehaviour
    {
        [Header("Text Elements")]
        [SerializeField] TMP_Text LargeGoldText;

        [Header("Prefabs")]
        [SerializeField] GameObject GoldTravelPS;

        [Header("Transforms")]
        [SerializeField] Transform GoldIconTransform;

        float _lastDisplayUpdate;

        int _numGoldTrails = 0;

        void Awake()
        {
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
            if (_numGoldTrails == 0 && Mathf.Abs(Time.timeSinceLevelLoad - _lastDisplayUpdate) > 0.25f)
            {
                UpdateDisplayGold();
            }
        }

        void UpdateDisplayGold()
        {
            _lastDisplayUpdate = Time.timeSinceLevelLoad;

            LargeGoldText.text  = Format.Number(App.Inventory.Gold, decimalPlaces: 1);
            //GoldText.text       = LargeGoldText.text;
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
    }
}
