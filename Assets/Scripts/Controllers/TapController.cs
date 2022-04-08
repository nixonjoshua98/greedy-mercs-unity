using GM.Common;
using GM.DamageTextPool;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GM.Controllers
{
    [RequireComponent(typeof(RectTransform))]
    public class TapController : GM.Core.GMMonoBehaviour, IPointerDownHandler
    {
        public ObjectPool ParticlePool;

        public int MaxTapsPerSecond = 20;

        private float ClickInterval => 1.0f / MaxTapsPerSecond;

        private Stopwatch stopWatch;
        private GameManager GameManager;
        private IDamageTextPool DamageNumberManager;

        // Events
        public UnityEvent E_OnTap = new();

        private void Start()
        {
            stopWatch = Stopwatch.StartNew();

            GameManager = this.GetComponentInScene<GameManager>();
            DamageNumberManager = this.GetComponentInScene<IDamageTextPool>();
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (stopWatch.ElapsedMilliseconds >= ClickInterval)
            {
                stopWatch.Restart();

                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);

                BigDouble damage = App.GMCache.TotalTapDamage;

                if (GameManager.DealDamageToTarget(damage, showDamageNumber: false))
                {
                    DamageNumberManager.Spawn(worldPosition, Format.Number(damage));
                }

                E_OnTap.Invoke();
            }

            SpawnClickEffect(eventData.position);
        }

        void SpawnClickEffect(Vector3 pos)
        {
            ClickObject inst = ParticlePool.Spawn<ClickObject>();

            inst.SetScreenPosition(new Vector3(pos.x, pos.y, 0));
        }
    }
}
