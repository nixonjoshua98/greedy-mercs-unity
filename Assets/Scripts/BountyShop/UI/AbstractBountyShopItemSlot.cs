using UnityEngine;

namespace SRC.BountyShop.UI
{
    public abstract class AbstractBountyShopItemSlot : SRC.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] protected GameObject PurchaseModalObject;

        GameObject _createdModal;

        private void OnDestroy()
        {
            if (_createdModal is not null)
            {
                Destroy(_createdModal);
            }
        }

        protected T InstantiateModal<T>(GameObject obj)
        {
            _createdModal = this.InstantiateUI(obj);

            return _createdModal.GetComponent<T>();
        }
    }
}
