using GM.Common.Enums;
using UnityEngine;

namespace GM.DamageTextPool
{
    public class DamageTextPool : MonoBehaviour
    {
        [SerializeField] private GM.Common.ObjectPool Pool;

        public void Spawn(Vector3 position, DamageType damageType, BigDouble value)
        {
            Spawn(position, Format.Number(value), damageType.TextColor());
        }

        public void Spawn(Vector3 position, string value, Color color)
        {
            var popup = Pool.Spawn<GM.UI.DamageNumberTextPopup>();

            popup.transform.position = Camera.main.WorldToScreenPoint(position);

            popup.Set(value, color);
        }

        public void Spawn(Vector3 position, string value)
        {
            var popup = Pool.Spawn<GM.UI.DamageNumberTextPopup>();

            popup.transform.position = Camera.main.WorldToScreenPoint(position);

            popup.SetValue(value);
        }
    }
}
