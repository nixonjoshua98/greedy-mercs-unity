using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace GreedyMercs
{
    public class DestroyWhenClicked : MonoBehaviour, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            GraphicRaycaster raycaster = Funcs.UI.MainCanvas.GetComponent<GraphicRaycaster>();

            List<RaycastResult> results = new List<RaycastResult>();

            raycaster.Raycast(eventData, results);

            if (results.Count >= 1 && results[0].gameObject == gameObject)
            {
                Destroy(gameObject);
            }
        }

        public void DestroyNow()
        {
            Destroy(gameObject);
        }
    }
}