﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs
{
    public class PlayerStatsCollection : MonoBehaviour
    {
        void Start()
        {
            Events.OnKillEnemy.AddListener(() => { GameState.Quests.enemyKills++; });
            Events.OnKilledBoss.AddListener(() => { GameState.Quests.bossKills++; });
            Events.OnPlayerClick.AddListener(() => { GameState.Quests.playerClicks++; });
            Events.OnPlayerPrestige.AddListener(() => { GameState.Quests.prestiges++; });
            Events.OnSkillActivated.AddListener(() => { GameState.Quests.skillsActivated++; });
        }
    }
}