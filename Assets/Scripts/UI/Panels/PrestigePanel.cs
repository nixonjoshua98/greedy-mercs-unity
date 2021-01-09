﻿using System.Numerics;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using SimpleJSON;

public class PrestigePanel : MonoBehaviour
{
    [SerializeField] Text prestigePointText;
    [SerializeField] Text bountyLevelsText;

    [SerializeField] RectTransform lootBagRect;

    [Header("Prefabs")]
    [SerializeField] GameObject ErrorMessage;

    void Awake()
    {
        InvokeRepeating("UpdatePanel", 0.1f, 0.1f);
    }

    void UpdatePanel()
    {
        bountyLevelsText.text = "Soon^tm";

        prestigePointText.text = Utils.Format.FormatNumber(StatsCache.GetPrestigePoints(GameState.Stage.stage));
    }

    public void Prestige()
    {
        if (GameState.Stage.stage < StageState.MIN_PRESTIGE_STAGE)
            return;

        JSONNode node = Utils.Json.GetDeviceNode();

        node.Add("prestigeStage", GameState.Stage.stage);

        SquadManager.ToggleAttacking(false);

        Server.Prestige(this, OnPrestigeCallback, node);
    }

    void OnPrestigeCallback(long code, string compressed)
    {
        if (code == 200)
        {
            DataManager.IsPaused = true;

            Utils.File.WriteJson(DataManager.LOCAL_FILENAME, Utils.Json.Decode(compressed));

            StartCoroutine(RunPrestigeAnimation());
        }

        else
        {
            Utils.UI.ShowError(ErrorMessage, "Server Connection", "Failed to contact the server :(");

            SquadManager.ToggleAttacking(true);
        }
    }

    IEnumerator RunPrestigeAnimation()
    {
        CancelInvoke("UpdatePanel");

        StartCoroutine(PanelAnimation(0.9f));

        yield return SquadManager.MoveOut(1.0f);

        bool _ = Utils.File.ReadJson(DataManager.LOCAL_FILENAME, out JSONNode node);

        GameState.Restore(node);

        SceneManager.LoadSceneAsync("GameScene");
    }

    IEnumerator PanelAnimation(float duration)
    {
        StartCoroutine(Utils.Lerp.RectTransform(lootBagRect, lootBagRect.localScale, lootBagRect.localScale * 1.25f, duration));

        BigInteger coins = StatsCache.GetPrestigePoints(GameState.Stage.stage);

        float progress = 0.0f;

        while (progress < 1.0f)
        {
            BigInteger temp = (coins.ToBigDouble() * (1 - progress)).ToBigInteger();

            prestigePointText.text = Utils.Format.FormatNumber(temp);

            progress += (Time.deltaTime / duration);

            yield return new WaitForEndOfFrame();
        }

        prestigePointText.text = "0";
    }
}