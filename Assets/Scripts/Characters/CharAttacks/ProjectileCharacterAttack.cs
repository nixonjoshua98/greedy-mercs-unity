using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Characters
{
    using GM.Projectiles;

    public class ProjectileCharacterAttack : AbstractCharacterAttack
    {
        [Header("Prefabs")]
        [SerializeField] GameObject ProjectileObject;

        [Header("References")]
        [SerializeField] Transform startPosition;

        [Header("Projectile Behaviour")]
        [SerializeField] float distance;
        [SerializeField] float speed;

        public override void Attack(GameObject obj)
        {
            base.Attack(obj);

            anim.Play(AttackAnimation);
        }

        public override void OnAttackAnimationEvent()
        {
            CreateProjectile();
        }

        public void CreateProjectile()
        {
            GameObject projectile = Instantiate(ProjectileObject, startPosition.position, Quaternion.identity, transform);

            projectile.GetComponent<StraightProjectile>().Init(OnProjectileHit, distance, speed);
        }

        public void OnProjectileHit()
        {
            OnAttackHit();
        }
    }
}
