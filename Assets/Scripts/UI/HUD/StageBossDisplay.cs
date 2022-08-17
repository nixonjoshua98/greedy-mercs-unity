using Coffee.UIEffects;
using TMPro;
using UnityEngine;

namespace SRC.UI.HUD
{
    public class StageBossDisplay : SRC.Core.GMMonoBehaviour
    {
        [SerializeField] private GameManager Manager;

        [SerializeField] private UIDissolve BossIncomingEffect;

        [Header("Text Elements")]
        [SerializeField] private TMP_Text BossText;
        [SerializeField] private TMP_Text FooterText;

        private void Awake()
        {
            BossIncomingEffect.effectFactor = 1;    // Hide the effect
        }

        private void Start()
        {
            Manager.E_OnPreBossReady.AddListener(OnBossSpawned);
            Manager.E_OnBossDefeated.AddListener(OnStageCompleted);
        }

        /* Callbacks */

        private void OnBossSpawned(SRC.Events.StageBossEventPayload payload)
        {
            BossText.text = payload.IsBounty ? App.Bounties.GetBountyForStage(payload.StageSpawned).Name : "Boss Battle";
            FooterText.text = $"--- Stage {payload.StageSpawned} Boss ---";

            BossIncomingEffect.Play(reset: true);
        }

        private void OnStageCompleted()
        {
            BossIncomingEffect.PlayReverse();
        }
    }
}
