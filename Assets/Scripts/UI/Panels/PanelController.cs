using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    [SerializeField] GameObject[] Frames;

    int CurrentFrame = 0;

    public void ToggleFrame(int index)
    {
        CurrentFrame = index;

        for (int i = 0; i < Frames.Length; ++i)
        {
            Frames[i].SetActive(i == (int)CurrentFrame);
        }
    }
}
