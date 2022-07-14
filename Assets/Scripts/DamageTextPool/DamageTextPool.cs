using GM.Enums;
using GM.Units;
using UnityEngine;

namespace GM.DamageTextPool
{
    public interface IDamageTextPool
    {
        void Spawn(Vector3 position, string value);
        void Spawn(GM.Units.UnitAvatar avatar, string value);

        void Spawn(UnitBase unit, BigDouble value, DamageType damageType);
    }


    public class DamageTextPool : MonoBehaviour, IDamageTextPool
    {
        [SerializeField] private GM.Common.ObjectPool Pool;

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

        public void Spawn(UnitBase unit, BigDouble value, DamageType damageType)
        {
            Spawn(unit.Avatar.Bounds.RandomCenterPosition(), Format.Number(value), damageType.TextColor());
        }

        private void Spawn(Vector3 position, string value, Color color)
        {
            GM.UI.DamageNumberTextPopup popup = Pool.Spawn<GM.UI.DamageNumberTextPopup>();

            popup.transform.position = Camera.main.WorldToScreenPoint(position);

            popup.Set(value, color);
        }
    }
}
