﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs.Characters
{
    using GM.Characters;

    public abstract class CharacterController : MonoBehaviour
    {
        [SerializeField] protected CharacterID character;

        [Header("Character Scripts")]
        protected CharacterAttack attack;

        protected virtual void Awake()
        {
            UpdateSortingLayers();
            GetInitialComponents();
        }

        protected virtual void Start()
        {
            attack.SetAttackCallback(OnAttackHit);
        }

        void FixedUpdate()
        {
            if (StartAttackCheck())
            {
                attack.StartAttack();
            }
        }

        protected abstract bool StartAttackCheck();

        protected virtual void OnAttackHit(float timeSinceAttack)
        {
            GameManager.TryDealDamageToEnemy(StatsCache.CharacterDamage(character) * timeSinceAttack);
        }

        void GetInitialComponents()
        {
            attack = GetComponent<CharacterAttack>();
        }

        void UpdateSortingLayers()
        {
            SpriteRenderer[] renderers = transform.GetComponentsInChildren<SpriteRenderer>();

            int maxSortingLayer = 0;

            foreach (SpriteRenderer sr in renderers)
            {
                sr.sortingOrder = 1000 - (int)transform.position.y;

                maxSortingLayer = Mathf.Max(sr.sortingOrder, maxSortingLayer);
            }

            UpdateSortingLayers(maxSortingLayer);
        }

        protected abstract void UpdateSortingLayers(int layer);

        // === Helper Methods ===
        public void Flip()
        {
            Vector3 scale = transform.localScale;

            scale.x *= -1.0f;

            transform.localScale = scale;
        }
    }
}