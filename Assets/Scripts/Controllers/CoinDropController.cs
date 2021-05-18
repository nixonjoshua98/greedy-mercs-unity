
using UnityEngine;

namespace GreedyMercs
{
    public class CoinDropController : MonoBehaviour
    {
        [SerializeField] ParticleSystem ps;

        public void Start()
        {
            Events.OnKillEnemy.AddListener(PlayCoinParticles);

            BossBattleManager.Instance.OnBossDeath.AddListener(PlayCoinParticles);
        }


        void PlayCoinParticles()
        {
            var emission = ps.emission;

            emission.rateOverTime = Mathf.Min(25, 5 + (2.5f * (GameState.Stage.stage / 10.0f)));

            ps.Play();
        }
    }
}