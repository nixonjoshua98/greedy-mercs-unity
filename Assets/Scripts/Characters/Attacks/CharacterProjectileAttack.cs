﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs
{
    public class CharacterProjectileAttack : CharacterAttack
    {
        [Header("Prefabs")]
        [SerializeField] protected GameObject ProjectileObject;

        [Header("References")]
        [SerializeField] protected Transform startPosition;

        [Header("Projectile Behaviour")]
        [SerializeField] float distance;
        [SerializeField] float speed;

        public override void OnAttackAnimationFinished()
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