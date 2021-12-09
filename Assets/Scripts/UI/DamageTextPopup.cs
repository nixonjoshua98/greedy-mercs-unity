using TMPro;
using UnityEngine;

namespace GM.UI
{
    public class DamageTextPopup : MonoBehaviour
    {
        [SerializeField] TMP_Text Text;

        public float lifetime = 0.5f;
        public float fadeDuration = 0.5f;
        [Space]
        public Vector2 moveVector = new Vector2(50, 125);

        float fadeTimer;

        Color originalColour;

        void Awake()
        {
            fadeTimer = fadeDuration;

            originalColour = Text.color;
        }


        public void Setup(string val)
        {
            Text.text = val;
        }

        public void Setup(string val, Vector3 position)
        {
            Setup(val);

            transform.position = position;
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


        protected virtual float ProcessFade()
        {
            fadeTimer -= Time.deltaTime;

            float percentFaded = fadeTimer / fadeDuration;

            Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, originalColour.a * percentFaded);

            return percentFaded;
        }
    }
}
