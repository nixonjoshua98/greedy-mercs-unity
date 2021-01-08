﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using SD = RotaryHeart.Lib.SerializableDictionary;

[System.Serializable]
public class NamedBossesDict : SD.SerializableDictionaryBase<int, ScriptableStageBoss> { }

public class BossBattleManager : MonoBehaviour
{
    static BossBattleManager Instance = null;

    bool _isAvoidingBoss = false;

    // Static public accessors
    public static bool IsAvoidingBoss { get { return Instance._isAvoidingBoss; } }
    // -----------------------

    [Header("UI Objects & Components")]
    [SerializeField] GameObject bossAnimObject;

    [SerializeField] Slider BossSlider;

    [SerializeField] GameObject BossButton;

    [Header("Text")]
    [SerializeField] Text BossTimerText;
    [SerializeField] Text bossNameText;

    [Header("Objects")]
    [SerializeField] Transform BossSpawnPoint;

    [Header("Bosses")]
    [SerializeField] GameObject[] BossObjects;
    [SerializeField] NamedBossesDict namedBosses;

    GameObject CurrentBossEnemy;

    void Awake()
    {
        Instance = this;

        SetUIActive(false);

        BossButton.SetActive(false);
    }

    void SetUIActive(bool active)
    {
        bossAnimObject.SetActive(active);

        BossSlider.gameObject.SetActive(active);
        BossTimerText.gameObject.SetActive(active);
    }

    // Static public method accessor
    public static void StartBossBattle()
    {
        // WARNING: GameManager sometimes calls this method twice for some reason,
        // so we have a check if a boss is already spawned and just ignore it if so.
        // This should be looked into but this *temporary* fix works well.
        // Does not work :(
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
        SetUIActive(true);

        GameObject bossToSpawn;

        bool isNamedBoss = namedBosses.TryGetValue(GameState.Stage.stage, out ScriptableStageBoss boss);

        if (isNamedBoss)
        {
            bossToSpawn = boss.prefab;

            bossNameText.text = boss.name.ToUpper();
        }
        // Normal boss
        else
        {
            bossNameText.text = "BOSS BATTLE";

            bossToSpawn = BossObjects[Random.Range(0, BossObjects.Length)];
        }

        CurrentBossEnemy = Instantiate(bossToSpawn, BossSpawnPoint.position, Quaternion.identity);

        EventManager.OnBossSpawned.Invoke(CurrentBossEnemy);

        yield return ITimer();

        SetUIActive(false);

        if (CurrentBossEnemy != null)
        {
            BossButton.SetActive(true);

            _isAvoidingBoss = true;

            Destroy(CurrentBossEnemy);

            EventManager.OnFailedToKillBoss.Invoke();
        }

        else
        {
            EventManager.OnKilledBoss.Invoke();
        }
    }

    IEnumerator ITimer()
    {
        float timer = 15.0f;

        BossSlider.value = timer;

        while (CurrentBossEnemy != null && timer > 0)
        {
            timer -= Time.deltaTime;

            BossSlider.value = timer;

            BossTimerText.text = Mathf.CeilToInt(timer).ToString();

            yield return new WaitForEndOfFrame();
        }
    }
}
