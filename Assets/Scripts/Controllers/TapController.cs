using GM.Common;
using GM.DamageTextPool;
using GM.Units;
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

        private Stopwatch _stopWatch;

        [SerializeField] UnitCollection EnemyUnits;
        private IDamageTextPool DamageNumberManager;

        [Header("Prefabs")]
        [HideInInspector] public UnityEvent E_OnTap = new();

        UnitBase CurrentTarget;

        void Awake()
        {
            _stopWatch = Stopwatch.StartNew();
        }

        private void Start()
        {
            DamageNumberManager = this.GetComponentInScene<IDamageTextPool>();
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            SpawnClickEffect(eventData.position);

            if (_stopWatch.ElapsedMilliseconds >= ClickInterval)
            {
                _stopWatch.Restart();

                if (EnemyUnits.Count == 0)
                    return;

                CurrentTarget = CurrentTarget == null ? EnemyUnits.First() : CurrentTarget;

                if (!Camera.main.IsVisible(CurrentTarget.transform.position))
                    return;

                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);

                BigDouble damage = App.GMCache.TotalTapDamage;

                HealthController health = CurrentTarget.GetComponent<HealthController>();

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
