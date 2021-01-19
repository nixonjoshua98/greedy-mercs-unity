﻿using System;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs
{
    using GreedyMercs.UI.Messages;

    public class ProgressCalculator : MonoBehaviour
    {
        [SerializeField] GameObject ProgressMessageObject;

        void Awake()
        {
            double processTime = (DateTime.UtcNow - GameState.LastLoginDate).TotalSeconds;

            processTime = Math.Min(24 * 60 * 60, processTime);

            if (processTime >= 60)
            {
                int stagesCompleted = Process(processTime, 100);

                if (stagesCompleted > 0)
                {
                    ProgressMessage msg = Utils.UI.Instantiate(ProgressMessageObject, Vector3.zero).GetComponent<ProgressMessage>();

                    msg.Init(stagesCompleted);
                }
            }

            Destroy(gameObject);
        }

        int Process(double progressTimeLeft, int maxStages)
        {
            BigDouble dps = StatsCache.TotalCharacterDPS;

            int stagesGained = 0;

            while (progressTimeLeft > 0 && stagesGained < maxStages)
            {
                BigDouble enemyHealth;

                bool isBoss = GameState.Stage.isStageCompleted;

                enemyHealth = isBoss ? Formulas.CalcBossHealth(GameState.Stage.stage) : Formulas.CalcEnemyHealth(GameState.Stage.stage);

                BigDouble secondsToKillEnemy = enemyHealth / dps;

                // We have enough time left to kil the enemy
                if (progressTimeLeft >= secondsToKillEnemy)
                {
                    if (isBoss)
                    {
                        // Takes too long to kill the boss
                        if (secondsToKillEnemy > StatsCache.StageBoss.Timer)
                            break;

                        stagesGained++;

                        GameState.Player.gold += StatsCache.GetBossGold(GameState.Stage.stage);

                        GameState.Stage.AdvanceStage();
                    }

                    else
                    {
                        GameState.Player.gold += StatsCache.GetEnemyGold(GameState.Stage.stage);

                        GameState.Stage.AddKill();
                    }

                    // Reduce the time it took to kill the enemy, as well as the global cooldown
                    progressTimeLeft -= (secondsToKillEnemy.ToDouble() + Formulas.EnemySpawnCooldown);

                    continue;
                }

                break;
            }

            return stagesGained;
        }
    }
}