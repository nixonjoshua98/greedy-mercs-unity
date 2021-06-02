using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreedyMercs.Characters
{
    public class StageCharacterController : CharacterController
    {
        [Header("Particles")]
        [SerializeField] ParticleSystem levelupParticles;

        protected override void Awake()
        {
            base.Awake();

            SubscribeToEvents();
        }

        protected override void Start()
        {
            base.Start();

            levelupParticles.Play();
        }

        // === Internal Methods ===
        void SubscribeToEvents()
        {
            Events.OnCharacterLevelUp.AddListener((CharacterID chara) => { if (chara == character) OnLevelup(); });
        }

        protected override void UpdateSortingLayers(int layer)
        {
            levelupParticles.GetComponent<ParticleSystemRenderer>().sortingOrder = layer + 1;
        }

        // === Events === 
        void OnLevelup()
        {
            levelupParticles.Play();
        }

        protected override bool StartAttackCheck()
        {
            return attack.IsAttackReady && GameManager.Instance.IsEnemyAvailable;
        }

        protected override void OnAttackHit(float timeSinceAttack)
        {
            GameManager.TryDealDamageToEnemy(StatsCache.TotalMercDamage(character) * timeSinceAttack);
        }
    }
}
