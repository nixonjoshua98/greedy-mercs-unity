using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.UI
{
    using TMPro;

    public class TextPopup : MonoBehaviour
    {
        public TMP_Text Text;

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


        public void Setup(string val, Color col)
        {
            Setup(val);

            Text.color = originalColour = col;
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


        float ProcessFade()
        {
            fadeTimer -= Time.deltaTime;

            float percentFaded = fadeTimer / fadeDuration;

            Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, originalColour.a * percentFaded);

            return percentFaded;
        }
    }
}
