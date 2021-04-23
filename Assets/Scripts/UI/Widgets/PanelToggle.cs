using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PanelToggle : MonoBehaviour
{
    [SerializeField] GameObject targetPanel;

    void Awake()
    {
        Toggle toggle = GetComponent<Toggle>();

        toggle.onValueChanged.AddListener(OnValueChanged);
    }

    void OnValueChanged(bool val)
    {
        targetPanel.SetActive(val);
    }
}
