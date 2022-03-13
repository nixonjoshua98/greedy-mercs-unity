using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GM.Navigation
{
    [System.Serializable]
    class TabStateSprites
    {
        public Color Selected = GM.Common.Constants.Colors.Orange;
        public Color Unselected = GM.Common.Constants.Colors.Grey;
    }

    public class TabButton : MonoBehaviour, IPointerClickHandler
    {
        public TabGroup Group;
        public bool InitiallySelected;

        [Space]
        [SerializeField] Image Background;
        [SerializeField] TabStateSprites Colours;

        public UnityEvent OnSelected;
        public UnityEvent OnDeselected;

        void Awake()
        {
            Group.Subscribe(this);
        }

        public void Select()
        {
            Background.color = Colours.Selected;

            OnSelected?.Invoke();
        }

        public void DeSelect()
        {
            Background.color = Colours.Unselected;

            OnDeselected?.Invoke();
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            Group.OnTabSelected(this);
        }
    }
}