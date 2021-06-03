using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GM
{
    public class StageBossBattleTimer : MonoBehaviour
    {
        [SerializeField] GameObject bossEntryAnimation;

        [SerializeField] Slider timerSlider;

        [SerializeField] Text timerText;

        // Events
        public UnityEvent OnTimerHitZero;

        bool _isBossAlive;

        void Start()
        {
            HideWidgets();

            // Temp. Derived events do not show in the inspector
            GetComponent<StageBossController>().OnBossSpawn.AddListener(OnBossSpawn);
        }

        public void OnBossSpawn(GameObject obj)
        {
            _isBossAlive = true;

            obj.GetComponent<Health>().OnDeath.AddListener(OnBossZeroHealth);

            StartCoroutine(BossLoop());

            bossEntryAnimation.SetActive(true);
        }

        void OnBossZeroHealth(GameObject obj)
        {
            _isBossAlive = false;

            HideWidgets();
        }

        void HideWidgets()
        {
            timerSlider.value   = 0;
            timerText.text      = "";

            bossEntryAnimation.SetActive(false);
        }

        IEnumerator BossLoop()
        {
            float maxTimer = StatsCache.StageEnemy.BossTimer;

            float timer = maxTimer;

            timerSlider.value = timer;

            while (timer > 0)
            {
                timer -= Time.deltaTime;

                timerSlider.value = timer / maxTimer;

                timerText.text = Mathf.CeilToInt(timer).ToString();

                yield return new WaitForEndOfFrame();

                if (!_isBossAlive)
                {
                    HideWidgets();

                    yield break;
                }
            }

            HideWidgets();

            OnTimerHitZero.Invoke();
        }
    }
}