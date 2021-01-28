using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GreedyMercs.UI
{
    public class PanelController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Image backgroundImage;

        [SerializeField] List<GameObject> tabs;

        public void ToggleActive(int index)
        {
            backgroundImage.enabled = !tabs[index].activeInHierarchy;

            for (int i = 0; i < tabs.Count; ++i)
            {
                tabs[i].SetActive(i == index && !tabs[i].activeInHierarchy);
            }
        }
    }
}