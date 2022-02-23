using UnityEngine;

namespace GM.UI
{
    public interface IDamageNumberManager
    {
        void InstantiateNumber(GM.Units.UnitAvatar avatar, BigDouble value);
    }

    public class DamageNumberManager : MonoBehaviour, IDamageNumberManager
    {
        [SerializeField] GM.Common.ObjectPool Pool;

        public void InstantiateNumber(GM.Units.UnitAvatar avatar, BigDouble value)
        {
            GM.UI.DamageNumberTextPopup popup = Pool.Spawn<GM.UI.DamageNumberTextPopup>();

            popup.transform.position = Camera.main.WorldToScreenPoint(avatar.Bounds.RandomCenterPosition());

            popup.SetValue(Format.Number(value));
        }
    }
}
