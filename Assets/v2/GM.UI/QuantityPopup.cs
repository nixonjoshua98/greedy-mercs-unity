using TMPro;
using UnityEngine;
using BigInteger = System.Numerics.BigInteger;

namespace GM.UI
{
    public class QuantityPopup : MonoBehaviour
    {
        [Header("References")]
        public TMP_Text QuantityText;

        [Header("Properties")]
        public float lifetime = 0.5f;
        public float fadeDuration = 0.5f;
        [Space]
        public Vector2 moveVector = new Vector2(0, 125);

        float fadeTimer;
        Color originalColour;

        public void Set(long val)
        {
            QuantityText.color = val > 0 ? Common.Colors.Gold : Common.Colors.Red;

            QuantityText.text = Format.Number(val);
        }

        public void Set(BigInteger val)
        {
            QuantityText.color = val > 0 ? Common.Colors.Gold : Common.Colors.Red;
            QuantityText.text = Format.Number(val);
        }

        void Awake()
        {
            fadeTimer = fadeDuration;

            originalColour = QuantityText.color;
        }


        void Update()
        {
            lifetime -= Time.deltaTime;

            UpdatePosition();

            if (lifetime <= 0.0f)
            {
                float percentFaded = ProcessFade();

                if (percentFaded <= 0.0f)
                {
                    Destroy(gameObject);
                }
            }
        }

        void UpdatePosition()
        {
            transform.position += moveVector.ToVector3() * Time.deltaTime;
        }


        protected float ProcessFade()
        {
            fadeTimer -= Time.deltaTime;

            float percentFaded = fadeTimer / fadeDuration;

            QuantityText.color = new Color(QuantityText.color.r, QuantityText.color.g, QuantityText.color.b, originalColour.a * percentFaded);

            return percentFaded;
        }
    }
}
