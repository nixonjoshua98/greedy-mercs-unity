using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TabController : MonoBehaviour
{
    [SerializeField] List<GameObject> tabs;

    public void ToggleActive(int index)
    {
        for (int i = 0; i < tabs.Count; ++i)
        {
            if (tabs[i].activeInHierarchy && index == i)
                tabs[i].SetActive(false);

            else
                tabs[i].SetActive(i == index);
        }
    }
}
