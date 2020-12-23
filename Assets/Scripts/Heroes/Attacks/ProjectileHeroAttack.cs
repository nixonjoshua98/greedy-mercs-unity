using System.Collections;

using UnityEngine;

public class ProjectileHeroAttack : HeroAttack
{
    [SerializeField] GameObject Projectile;
    [SerializeField] Transform ProjectileStart;

    public override void OnAttackAnimationEnd()
    {
        StartCoroutine(FireProjectile());
    }

    void OnProjectileFinished()
    {
        DealDamage();
    }

    IEnumerator FireProjectile()
    {
        Vector3 direction = Vector3.right * 4;

        GameObject projectile = Instantiate(Projectile, transform);

        float progress = 0.0f;

        while (progress < 1.0f)
        {
            progress = Mathf.Clamp01(progress + (Time.fixedDeltaTime * 2));

            projectile.transform.position = Vector3.Lerp(ProjectileStart.position, ProjectileStart.position + direction, progress);

            yield return new WaitForFixedUpdate();
        }

        OnProjectileFinished();

        Destroy(projectile);
    }
}
