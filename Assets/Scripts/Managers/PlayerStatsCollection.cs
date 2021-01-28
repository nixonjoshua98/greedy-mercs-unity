using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs
{
    public class PlayerStatsCollection : MonoBehaviour
    { 
        void Start()
        {
            Events.OnKillEnemy.AddListener(OnEnemyDeath);
            Events.OnKilledBoss.AddListener(OnBossDeath);
            Events.OnPlayerClick.AddListener(OnPlayerClick);
        }

        void OnEnemyDeath()
        {
            GameState.Quests.enemyKills++;
        }

        void OnBossDeath()
        {
            GameState.Quests.bossKills++;
        }

        void OnPlayerClick()
        {
            GameState.Quests.playerClicks++;
        }
    }
}