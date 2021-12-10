using UnityEngine;
using UnityEngine.UI;

namespace GM.UI
{
    public class DestroyButton : MonoBehaviour
    {
        public GameObject TargetObject;

        Button Button;

        void Awake()
        {
            Button = GetComponent<Button>();

            Button.onClick.AddListener(OnClick);
        }

        public bool interactable
        {
            get => Button.interactable;
            set => Button.interactable = value;
        }

        void OnClick()
        {
            Destroy(TargetObject);
            Destroy(gameObject);
        }
    }
}
