using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GM.Scenes
{
    public enum SceneTransitionType
    {
        INTRO = 0,
        OUTRO = 1
    }


    public class SceneTransition : MonoBehaviour
    {
        [SerializeField] Image image;

        [SerializeField] SceneTransitionType transition;

        float TransitionSpeed = 1;

        public UnityEvent E_OnFinished { get; private set; } = new UnityEvent();

        void Awake()
        {
            image.fillAmount = transition == SceneTransitionType.INTRO ? 1 : 0;
        }


        void Update()
        {
            float ts = Mathf.Min(1 / 60.0f, Time.unscaledDeltaTime);

            int multiplier = transition == SceneTransitionType.INTRO ? -1 : 1;

            image.fillAmount += (ts / TransitionSpeed) * multiplier;

            if (Mathf.Abs(image.fillAmount) == 0.0f || Mathf.Abs(image.fillAmount) == 1.0f)
            {
                E_OnFinished.Invoke();

                if (transition == SceneTransitionType.INTRO)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
