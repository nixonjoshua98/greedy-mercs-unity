using System.Collections;
using System.Collections.Generic;

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

    protected override void OnChangeWeapon(ScriptableWeapon weapon)
    {
        GameObject newParticleSystem = Instantiate(weapon.prefab, transform);

        newParticleSystem.transform.position = ps.gameObject.transform.position;

        Destroy(ps.gameObject);

        ps = newParticleSystem.GetComponent<ParticleSystem>();
    }
}
