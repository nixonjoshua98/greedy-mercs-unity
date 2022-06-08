using UnityEngine;

namespace GM.BountyShop.UI
{
    public abstract class AbstractBountyShopItemSlot : GM.Core.GMMonoBehaviour
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
