
using UnityEngine;

namespace GreedyMercs
{
    public class CharacterParticleAttack : CharacterAttack
    {
        [Header("Components")]
        [SerializeField] ParticleSystem ps;

        [Header("References")]
        [SerializeField] GameObject AttackSlot;

        public override void OnAttackEvent()
        {
            ps.Play();
        }

        public void OnParticleDone()
        {
            DealDamage();
        }

        protected override void OnChangeWeapon(WeaponSO weapon)
        {

        }
    }
}