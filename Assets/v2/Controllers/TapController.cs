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
        public int MaxTapsPerSecond = 20;
        float ClickInterval { get => 1.0f / MaxTapsPerSecond; }

        Stopwatch stopWatch;

        GameManager GameManager;
        IDamageTextPool DamageNumberManager;

        // Events
        public UnityEvent E_OnTap = new();

        void Start()
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
        }
    }
}
