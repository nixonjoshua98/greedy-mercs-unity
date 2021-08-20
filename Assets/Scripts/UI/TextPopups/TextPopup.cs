using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.UI
{
    using TMPro;

    public class TextPopup : MonoBehaviour
    {
        TMP_Text Text;

        float lifetime = 0.5f;
        float fadeDuration = 0.5f;

        float fadeTimer;

        Color originalColour;

        void Awake()
        {
            Text = GetComponent<TMP_Text>();

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
            transform.position += new Vector3(50, 125) * Time.deltaTime;
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
