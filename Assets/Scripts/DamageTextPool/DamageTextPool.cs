using SRC.Common.Enums;
using UnityEngine;

namespace SRC.DamageTextPool
{
    public class DamageTextPool : MonoBehaviour
    {
        [SerializeField] private SRC.Common.ObjectPool Pool;

        public void Spawn(Vector3 position, DamageType damageType, BigDouble value)
        {
            Spawn(position, Format.Number(value), damageType.TextColor());
        }

        public void Spawn(Vector3 position, string value, Color color)
        {
            var popup = Pool.Spawn<SRC.UI.DamageNumberTextPopup>();

            popup.transform.position = Camera.main.WorldToScreenPoint(position);

            popup.Set(value, color);
        }

        public void Spawn(Vector3 position, string value)
        {
            var popup = Pool.Spawn<SRC.UI.DamageNumberTextPopup>();

            popup.transform.position = Camera.main.WorldToScreenPoint(position);

            popup.SetValue(value);
        }
    }
}
