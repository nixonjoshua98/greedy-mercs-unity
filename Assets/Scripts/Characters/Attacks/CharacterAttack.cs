using System.Collections;
using System.Collections.Generic;

using UnityEngine;


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

        // === Call the callback for the intial weapon change at creation ===
        ScriptableCharacter chara = CharacterResources.Instance.GetCharacter(character.CharacterID);

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

    void OnWeaponBought(ScriptableCharacter chara, int weaponIndex)
    {
        if (chara.character == character.CharacterID)
        {
            ScriptableWeapon weapon = chara.weapons[weaponIndex];

            int highestWeapon = GameState.Weapons.GetHighestTier(chara.character);

            if (weaponIndex > highestWeapon)
                OnChangeWeapon(weapon);
        }
    }

    protected abstract void OnChangeWeapon(ScriptableWeapon weapon);

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
