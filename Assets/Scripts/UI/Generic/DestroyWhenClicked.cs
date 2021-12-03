using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace GM
{
    public class DestroyWhenClicked : Core.GMMonoBehaviour, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            GraphicRaycaster raycaster = GameObject.Find("MainCanvas").GetComponent<GraphicRaycaster>();

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