using TMPro;
using UnityEngine;
using BigInteger = System.Numerics.BigInteger;


namespace GM.UI
{
    public class TextPopup : MonoBehaviour
    {
        [Header("Components")]
        public TMP_Text Text;

        [Header("Properties")]
        [SerializeField] Vector2 MinMoveVector = new Vector2(0, 125);
        [SerializeField] Vector2 MaxMoveVector = new Vector2(0, 125);
        [Space]
        public float Lifetime = 0.5f;
        public float FadeDuration = 0.5f;

        // ...
        Vector2 MoveVector;
        Color OriginalColour;

        float fadeTimer;
        float lifetimeTimer;

        bool isSet = false;

        void Awake()
        {
            OriginalColour = Text.color;
        }

        public void SetValue(string value)
        {
            Reset();

            Text.text = value;
            Text.color = OriginalColour;
        }

        public void Set(BigInteger val)
        {
            Reset();

            Text.text = Format.Number(val);
            Text.color = val > 0 ? GM.Common.Colors.Gold : GM.Common.Colors.Red;
        }

        public void Set(BigDouble val)
        {
            Reset();

            Text.text = Format.Number(val);
            Text.color = val > 0 ? GM.Common.Colors.Gold : GM.Common.Colors.Red;
        }

        protected void Reset()
        {
            isSet = true;
            lifetimeTimer = Lifetime;
            fadeTimer = FadeDuration;
            Text.color = OriginalColour;

            MoveVector = new Vector2(Random.Range(MinMoveVector.x, MaxMoveVector.x), Random.Range(MinMoveVector.y, MaxMoveVector.y));
        }

        void Update()
        {
            if (isSet)
            {
                lifetimeTimer -= Time.deltaTime;

                transform.position += new Vector3(MoveVector.x, MoveVector.y) * Time.deltaTime;

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

        protected void OnFinished()
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
