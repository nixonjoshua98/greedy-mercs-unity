﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class BossBattleManager : MonoBehaviour
{
    static BossBattleManager Instance = null;

    bool _isAvoidingBoss = false;

    // Static public accessors
    public static bool IsAvoidingBoss { get { return Instance._isAvoidingBoss; } }
    // -----------------------

    [Header("UI Objects & Components")]
    [SerializeField] Slider BossSlider;

    [SerializeField] GameObject BossButton;
    [SerializeField] GameObject BossText;

    [Header("Objects")]
    [SerializeField] Transform BossSpawnPoint;
    [Space]
    [SerializeField] GameObject[] BossObjects;

    GameObject CurrentBossEnemy;

    void Awake()
    {
        Instance = this;

        BossSlider.gameObject.SetActive(false);
        BossButton.gameObject.SetActive(false);
        BossText.gameObject.SetActive(false);
    }

    // Static public method accessor
    public static void StartBossBattle()
    {
        // WARNING: GameManager sometimes calls this method twice for some reason,
        // so we have a check if a boss is already spawned and just ignore it if so.
        // This should be looked into but this *temporary* fix works well.
        // This didn't work :(
        if (Instance.CurrentBossEnemy == null)
        {
            Instance.StartCoroutine(Instance.IBossBattle());
        }
    }

    public void OnFightBossButton()
    {
        _isAvoidingBoss = false;

        GameManager.TrySkipToBoss();

        BossButton.gameObject.SetActive(false);
    }

    IEnumerator IBossBattle()
    {
        BossText.gameObject.SetActive(true);

        CurrentBossEnemy = Instantiate(BossObjects[Random.Range(0, BossObjects.Length)], BossSpawnPoint.position, Quaternion.identity);

        EventManager.OnBossSpawned.Invoke(CurrentBossEnemy);

        yield return ITimer();

        BossText.gameObject.SetActive(false);

        if (CurrentBossEnemy != null)
        {
            // Active button to skip to the boss
            BossButton.gameObject.SetActive(true);

            _isAvoidingBoss = true;

            Destroy(CurrentBossEnemy);

            EventManager.OnFailedToKillBoss.Invoke();
        }
    }

    IEnumerator ITimer()
    {
        float timer = 15.0f;

        BossSlider.gameObject.SetActive(true);

        BossSlider.value = timer;

        while (CurrentBossEnemy != null && timer > 0)
        {
            timer -= Time.deltaTime;

            BossSlider.value = timer;

            yield return new WaitForEndOfFrame();
        }

        BossSlider.gameObject.SetActive(false);
    }
}