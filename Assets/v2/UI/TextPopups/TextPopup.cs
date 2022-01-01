using TMPro;
using UnityEngine;
using BigInteger = System.Numerics.BigInteger;

namespace GM.UI
{
    public class TextPopup : MonoBehaviour
    {
        public TMP_Text Text;

        [Header("Properties")]
        public float Lifetime = 0.5f;
        public float FadeDuration = 0.5f;
        [Space]
        public Vector2 MoveVector = new Vector2(0, 125);

        float fadeTimer;
        float lifetimeTimer;

        bool isSet = false;

        public void Set(BigInteger val)
        {
            Reset();

            Text.text = Format.Number(val);
            Text.color = val > 0 ? Common.Colors.Gold : Common.Colors.Red;
        }

        public void Set(BigDouble val)
        {
            Reset();

            Text.text = Format.Number(val);
            Text.color = val > 0 ? Common.Colors.Gold : Common.Colors.Red;
        }

        public void Set(BigDouble val, Color color, Vector3 position)
        {
            Reset();

            Text.text = Format.Number(val);
            Text.color = color;

            transform.position = position;
        }

        void Reset()
        {
            isSet = true;
            lifetimeTimer = Lifetime;
            fadeTimer = FadeDuration;
        }

        void Update()
        {
            if (isSet)
            {
                lifetimeTimer -= Time.deltaTime;

                transform.position += MoveVector.ToVector3() * Time.deltaTime;

                if (lifetimeTimer <= 0.0f)
                {
                    float percentFaded = ProcessFade();

                    if (percentFaded <= 0.0f)
                    {
                        OnFinished();
                    }
                }
            }
        }

        protected virtual void OnFinished()
        {
            isSet = false;

            gameObject.SetActive(false);
        }


        protected float ProcessFade()
        {
            fadeTimer -= Time.deltaTime;

            float percentFaded = fadeTimer / FadeDuration;

            Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, percentFaded);

            return percentFaded;
        }
    }
}
