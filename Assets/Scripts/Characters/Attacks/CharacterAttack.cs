using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using CharacterData;

[RequireComponent(typeof(Character))]
public abstract class CharacterAttack : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] Character character;

    [Header("Components")]
    [SerializeField] protected Animator anim;

    public Animator Anim { get { return anim; } }

    [Header("Properties")]
    [SerializeField, Range(0, 2.5f)] float delayBetweenAttacks = 0.25f;

    float attackTimer;

    float lastAttackTime;

    bool isAttacksToggled;
    
    protected bool CanAttack { get { return isAttacksToggled && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"); } }

    void Awake()
    {
        ToggleAttacking(true);

        attackTimer = delayBetweenAttacks;

        CharacterSO chara = StaticData.Chars.Get(character.CharacterID);

        if (chara.weapons.Length > 1)
        {
            int highestWeapon = GameState.Weapons.GetHighestTier(character.CharacterID);

            OnChangeWeapon(chara.weapons[highestWeapon]);
        }

        Events.OnWeaponBought.AddListener(OnWeaponBought);
    }

    void Start()
    {
        lastAttackTime = Time.realtimeSinceStartup;

        attackTimer = delayBetweenAttacks;
    }

    void FixedUpdate()
    {
        if (CanAttack)
        {
            attackTimer -= Time.fixedDeltaTime;

            if (attackTimer <= 0.0f)
            {
                attackTimer = delayBetweenAttacks;

                if (GameManager.IsEnemyAvailable)
                {
                    StartAttack();
                }
            }
        }
    }

    public abstract void OnAttackEvent();

    void OnWeaponBought(CharacterSO chara, int weaponIndex)
    {
        if (chara.CharacterID == character.CharacterID)
        {
            WeaponSO weapon = chara.weapons[weaponIndex];

            int highestWeapon = GameState.Weapons.GetHighestTier(chara.CharacterID);

            if (weaponIndex > highestWeapon)
                OnChangeWeapon(weapon);
        }
    }

    protected abstract void OnChangeWeapon(WeaponSO weapon);

    void StartAttack()
    {
        anim.Play("Attack");
    }

    protected void DealDamage()
    {
        float timeSinceAttack = Time.realtimeSinceStartup - lastAttackTime;

        GameManager.TryDealDamageToEnemy(StatsCache.GetCharacterDamage(character.CharacterID) * (timeSinceAttack * Time.timeScale));

        lastAttackTime = Time.realtimeSinceStartup;
    }

    public void ToggleAttacking(bool val)
    {
        isAttacksToggled = val;
    }
}
