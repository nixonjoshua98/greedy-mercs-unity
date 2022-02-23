
using GM.Units;
using System;
using UnityEngine;
using GM.Units.Projectiles;

namespace GM.Mercs.Controllers
{
    //public class ProjectileAttackController : AttackController
    //{
    //    public UnitAvatar Avatar;

    //    [Header("Prefabs")]
    //    public GameObject ProjectileObject;

    //    [Space]

    //    public Transform ProctileSpawnPosition;

    //    [Header("Properties")]
    //    public float AttackRangeY = 0.5f;
    //    public float AttackRangeX = 2.0f;

    //    // = Controllers = //
    //    IMovementController MoveController;

    //    void Awake()
    //    {
    //        SetupEvents();
    //        GetComponents();
    //    }

    //    void SetupEvents()
    //    {
    //        var events = GetComponentInChildren<UnitAvatarAnimationEvents>();

    //        events.Attack.AddListener(OnAttackAnimation);
    //    }

    //    void GetComponents()
    //    {
    //        MoveController = GetComponent<IMovementController>();

    //        GMLogger.WhenNull(MoveController, "IMovementController is Null");
    //    }

    //    public override bool InAttackPosition(Target target)
    //    {
    //        Vector3 pos = GetTargetPositionFromTarget(target);

    //        return InAttackPositionY(pos) && InAttackPositionX(pos);
    //    }

    //    bool InAttackPositionY(Vector3 target) => Mathf.Abs(target.y - transform.position.y) < AttackRangeY;
    //    bool InAttackPositionX(Vector3 target) => Mathf.Abs(target.x - transform.position.x) < AttackRangeX;


    //    public override void MoveTowardsAttackPosition(GM.Units.UnitBaseClass target)
    //    {
    //        Vector3 position = GetTargetPositionFromTarget(target);

    //        if (InAttackPositionX(position))
    //        {
    //            position.x = transform.position.x;
    //        }

    //        MoveController.MoveTowards(position);
    //        MoveController.LookAt(target.transform.position);
    //    }

    //    public override void StartAttack(GM.Units.UnitBaseClass target, Action<GM.Units.UnitBaseClass> callback)
    //    {
    //        base.StartAttack(target, callback);

    //        Avatar.PlayAnimation(Avatar.AnimationStrings.Attack);
    //    }


    //    Vector3 GetTargetPositionFromTarget(GM.Units.UnitBaseClass target)
    //    {
    //        return target.transform.position;
    //    }


    //    void InstantiateProjectile()
    //    {
    //        //IProjectile projectile = Instantiate<IProjectile>(ProjectileObject, ProctileSpawnPosition.position);

    //        //projectile.Init(CurrentTarget, OnProjectileImpact);
    //    }

    //    void OnProjectileImpact()
    //    {
    //        DealDamageToTarget();
    //    }

    //    void OnAttackAnimation()
    //    {
    //        if (IsTargetValid())
    //        {
    //            InstantiateProjectile();
    //        }

    //        Cooldown();
    //    }
    //}
}