using UnityEngine;
using UnityEngine.UI;

namespace GM.UI
{
    public class DestroyButton : MonoBehaviour
    {
        public GameObject TargetObject;

        void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                Destroy(TargetObject);
                Destroy(gameObject);
            });
        }
    }
}
