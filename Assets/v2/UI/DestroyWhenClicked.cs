using GM.Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GM.UI
{
    public class DestroyWhenClicked : MonoBehaviour, IPointerDownHandler
    {
        GraphicRaycaster CanvasRaycaster;

        void Awake()
        {
            GameObject canvas = GameObject.FindGameObjectWithTag(Constants.Tags.MainCanvas);

            CanvasRaycaster = canvas?.GetComponent<GraphicRaycaster>();

            GMLogger.WhenNull(CanvasRaycaster, "CanvasRaycaster not found");
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            List<RaycastResult> results = new List<RaycastResult>();

            CanvasRaycaster.Raycast(eventData, results);

            if (results.Count >= 1 && results[0].gameObject == gameObject)
            {
                Destroy(gameObject);
            }
        }

        void Destroy()
        {
            Destroy(gameObject);
        }
    }

}
