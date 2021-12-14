
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
            ClickObject inst = ParticlePool.Spawn<ClickObject>();

            inst.SetScreenPosition(new Vector3(pos.x, pos.y, 0));

            inst.PS.Play();
        }
    }
}