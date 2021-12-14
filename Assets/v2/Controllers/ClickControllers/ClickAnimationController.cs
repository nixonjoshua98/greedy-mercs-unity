using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Controllers
{
    public class ClickAnimationController : AbstractClickController
    {
        static ClickAnimationController Instance = null;

        [Header("References")]
        public Animator ClickAnimatorObject;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                DontDestroyOnLoad(Instance.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected override void OnClick(Vector3 pos)
        {
            
        }
    }
}