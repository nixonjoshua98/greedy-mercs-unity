using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ErrorMessage : MonoBehaviour
{
    public Text Title;
    public Text Description;

    public void OnClose()
    {
        Destroy(gameObject);
    }
}