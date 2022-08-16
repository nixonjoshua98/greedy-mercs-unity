using Coffee.UIEffects;
using TMPro;
using UnityEngine;

namespace SRC.UI.HUD
{
    public class StageBossDisplay : SRC.Core.GMMonoBehaviour
    {
        [SerializeField] GameManager Manager;

        [SerializeField] UIDissolve BossIncomingEffect;

        [Header("Text Elements")]
        [SerializeField] TMP_Text BossText;
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

        void OnBossSpawned(SRC.Events.StageBossEventPayload payload)
        {
            BossText.text   = payload.IsBounty ? App.Bounties.GetBountyForStage(payload.StageSpawned).Name : "Boss Battle";
            FooterText.text = $"--- Stage {payload.StageSpawned} Boss ---";

            BossIncomingEffect.Play(reset: true);
        }

        void OnStageCompleted()
        {
            BossIncomingEffect.PlayReverse();
        }
    }
}
