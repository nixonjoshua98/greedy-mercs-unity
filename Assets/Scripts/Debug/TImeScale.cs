using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

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
