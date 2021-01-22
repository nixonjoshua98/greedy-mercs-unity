using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs
{
    public class SatyrAttack : CharacterProjectileAttack
    {
        public override void CreateProjectile()
        {
            GameObject projectile = Instantiate(ProjectileObject, startPosition.position, Quaternion.identity);

            projectile.GetComponent<StraightProjectile>().Init(OnProjectileHit, 4.0f);
        }
    }
}