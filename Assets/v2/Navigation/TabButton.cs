using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GM.Navigation
{
    [System.Serializable]
    struct TabStateSprites
    {
        public Color Selected;
        public Color Unselected;
    }

    public class TabButton : MonoBehaviour, IPointerClickHandler
    {
        public TabGroup Group;
        public bool InitiallySelected;

        [Space]
        [SerializeField] Image Background;
        [SerializeField] TabStateSprites Colours;

        void Awake()
        {
            Group.Subscribe(this);
        }

        public void Select()
        {
            Background.color = Colours.Selected;
        }

        public void DeSelect()
        {
            Background.color = Colours.Unselected;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            Group.OnTabSelected(this);
        }
    }
}