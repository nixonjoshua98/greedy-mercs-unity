using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SRC.Navigation
{
    [System.Serializable]
    internal class TabStateSprites
    {
        public Color Selected = SRC.Common.Constants.Colors.Orange;
        public Color Unselected = SRC.Common.Constants.Colors.Grey;
    }

    public class TabButton : MonoBehaviour, IPointerClickHandler
    {
        public TabGroup Group;
        public bool InitiallySelected;

        [Space]
        [SerializeField] private Image Background;
        [SerializeField] private TabStateSprites Colours;

        public UnityEvent E_OnSelected;
        public UnityEvent E_OnDeselected;

        private void Awake()
        {
            Group.Subscribe(this);
        }

        public void Select()
        {
            Background.color = Colours.Selected;

            E_OnSelected?.Invoke();
        }

        public void DeSelect()
        {
            Background.color = Colours.Unselected;

            E_OnDeselected?.Invoke();
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            Group.OnTabSelected(this);
        }
    }
}