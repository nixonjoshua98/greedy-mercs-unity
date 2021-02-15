
using UnityEngine;

namespace GreedyMercs
{
    public class CharacterParticleAttack : CharacterAttack
    {
        [Header("Components")]
        [SerializeField] ParticleSystem ps;

        public override void OnAttackAnimationFinished()
        {
            ps.Play();
        }

        public void OnParticleDone()
        {
            OnAttackHit();
        }
    }
}