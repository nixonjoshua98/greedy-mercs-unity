using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.UI
{
    using TMPro;

    public class DamagePopup : MonoBehaviour
    {
        #region Static
        static GameObject _PopupObject;

        public static GameObject PopupObject
        {
            get
            {
                if (_PopupObject == null)
                    _PopupObject = Resources.Load<GameObject>("DamagePopup_DEFAULT");

                return _PopupObject;
            }
        }

        public static GameObject Create(Vector2 position, string value, bool worldPosition)
        {
            if (worldPosition)
                position = WorldToScreen(position);

            GameObject o = CanvasUtils.Instantiate(PopupObject, position);

            o.GetComponent<DamagePopup>().Setup(value);

            return o;
        }

        static Vector2 WorldToScreen(Vector2 pos)
        {
            Vector3 temp = Camera.main.WorldToScreenPoint(pos);

            return new Vector2(temp.x, temp.y);
        }
        #endregion

        [SerializeField] TMP_Text text;

        float lifetime = 0.5f;
        float fadeDuration = 0.5f;

        float fadeTimer;

        Color originalColour;

        void Awake()
        {
            fadeTimer = fadeDuration;

            originalColour = text.color;
        }


        void Setup(string val)
        {
            text.SetText(val);
        }

        void Update()
        {
            transform.position += new Vector3(0, 25.0f) * Time.deltaTime;

            lifetime -= Time.deltaTime;

            if (lifetime <= 0.0f)
            {
                float fadedPercent = ProcessFade();

                if (fadedPercent <= 0.0f)
                    Destroy(gameObject);
            }
        }

        float ProcessFade()
        {
            fadeTimer -= Time.deltaTime;

            float percentFaded = fadeTimer / fadeDuration;

            text.color = new Color(text.color.r, text.color.g, text.color.b, originalColour.a * percentFaded);

            return percentFaded;
        }
    }
}
