using UnityEngine;

namespace GM.UI
{
    public interface IDamageNumberManager
    {
        void Spawn(Vector3 position, string value);
        void Spawn(GM.Units.UnitAvatar avatar, string value);
    }


    public class DamageNumberManager : MonoBehaviour, IDamageNumberManager
    {
        [SerializeField] GM.Common.ObjectPool Pool;

        public void Spawn(Vector3 position, string value)
        {
            GM.UI.DamageNumberTextPopup popup = Pool.Spawn<GM.UI.DamageNumberTextPopup>();

            popup.transform.position = Camera.main.WorldToScreenPoint(position);

            popup.SetValue(value);
        }

        public void Spawn(GM.Units.UnitAvatar avatar, string value)
        {
            Spawn(avatar.Bounds.RandomCenterPosition(), value);
        }
    }
}
