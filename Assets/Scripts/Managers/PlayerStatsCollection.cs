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
        }

        void OnEnemyDeath()
        {
            GameState.Quests.enemyKills++;
        }
    }
}