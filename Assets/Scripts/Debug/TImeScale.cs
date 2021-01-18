
using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.Debug
{
    public class TimeScale : MonoBehaviour
    {
        Slider slider;

        void Awake()
        {
            slider = GetComponent<Slider>();
        }

        public void OnSliderValueChanged()
        {
            Time.timeScale = slider.value;
        }
    }
}