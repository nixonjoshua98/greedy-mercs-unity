using Coffee.UIEffects;
using UnityEngine;
using TMPro;

namespace GM.UI.HUD
{
    public class StageBossDisplay : GM.Core.GMMonoBehaviour
    {
        [SerializeField] GameManager Manager;

        [SerializeField] UIDissolve BossIncomingEffect;

        [Header("Text Elements")]
        [SerializeField] TMP_Text FooterText;

        void Awake()
        {
            BossIncomingEffect.effectFactor = 1;    // Hide the effect
        }

        void Start()
        {
            Manager.E_OnPreBossReady.AddListener(OnBossSpawned);
            Manager.E_OnBossDefeated.AddListener(OnStageCompleted);
        }

        /* Callbacks */

        void OnBossSpawned(GM.Events.StageBossEventPayload payload)    
        {
            FooterText.text = $"--- Stage {payload.StageSpawned} Boss ---";

            BossIncomingEffect.Play(reset: true);
        }

        void OnStageCompleted()
        {
            BossIncomingEffect.PlayReverse();
        }
    }
}
