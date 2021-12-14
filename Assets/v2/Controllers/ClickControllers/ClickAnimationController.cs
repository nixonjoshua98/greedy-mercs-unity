
using UnityEngine;

namespace GM.Controllers
{
    public class ClickAnimationController : AbstractClickController
    {
        static ClickAnimationController Instance = null;

        [Header("References")]
        public GameObject ClickPS;

        [Space]
        public GM.Common.ObjectPool ParticlePool;

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
            pos = Camera.main.ScreenToWorldPoint(pos);

            ParticleSystem inst = ParticlePool.Spawn<ParticleSystem>(new Vector3(pos.x, pos.y, 0));

            inst.Play();
        }
    }
}