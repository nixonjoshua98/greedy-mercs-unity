
using UnityEngine;

public class CharacterParticleAttack : CharacterAttack
{
    [SerializeField] ParticleSystem ps;

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
