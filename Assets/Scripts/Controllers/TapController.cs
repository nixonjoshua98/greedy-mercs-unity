using GM.Common;
using GM.DamageTextPool;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GM.Controllers
{
    [RequireComponent(typeof(RectTransform))]
    public class TapController : GM.Core.GMMonoBehaviour, IPointerDownHandler
    {
        public GameManager Manager;

        public ObjectPool ParticlePool;

        public int MaxTapsPerSecond = 20;
        private float ClickInterval => 1.0f / MaxTapsPerSecond;

        private Stopwatch _stopWatch;

        private GM.DamageTextPool.DamageTextPool DamageNumberManager;

        [Header("Prefabs")]
        [HideInInspector] public UnityEvent E_OnTap = new();

        void Awake()
        {
            _stopWatch = Stopwatch.StartNew();
        }

        private void Start()
        {
            DamageNumberManager = this.GetComponentInScene<GM.DamageTextPool.DamageTextPool>();
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            SpawnClickEffect(eventData.position);

            if (_stopWatch.ElapsedMilliseconds >= ClickInterval)
            {
                _stopWatch.Restart();

                if (Manager.EnemyUnits.Count == 0)
                {
                    return;
                }

                var target = Manager.EnemyUnits.First();

                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);

                BigDouble damage = App.Values.TotalTapDamage;

                HealthController health = target.GetComponent<HealthController>();

                health.TakeDamage(damage);

                DamageNumberManager.Spawn(worldPosition, Format.Number(damage));

                E_OnTap.Invoke();
            }
        }

        void SpawnClickEffect(Vector3 pos)
        {
            ClickObject inst = ParticlePool.Spawn<ClickObject>();

            inst.SetScreenPosition(new Vector3(pos.x, pos.y, 0));
        }
    }
}
