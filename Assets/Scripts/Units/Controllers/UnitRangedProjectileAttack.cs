
using UnityEngine;

namespace GM.Units
{
    using GM.Projectiles;

    public class UnitRangedProjectileAttack : UnitRangedAttack
    {
        [Header("Projectile")]
        [SerializeField] GameObject projectile;
        [SerializeField] Transform projectileSpawn;

        protected override void OnAttackAnimation()
        {
            InstantiateProjectile();
        }


        void OnProjectileConnected()
        {
            E_OnAttackImpact.Invoke(currentTarget);
        }


        private void InstantiateProjectile()
        {
            GameObject o = Instantiate(projectile, projectileSpawn.transform.position, Quaternion.identity);

            IProjectile script = o.GetComponent<IProjectile>();

            script.Setup(currentTarget.transform, OnProjectileConnected);
        }
    }
}
