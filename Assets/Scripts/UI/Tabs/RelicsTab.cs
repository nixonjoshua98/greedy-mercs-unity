
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using SimpleJSON;

public class RelicsTab : MonoBehaviour
{
    [SerializeField] Text PrestigePointText;
    [SerializeField] Text PrestigeButtonText;
    [Space]
    [SerializeField] GameObject BlankPanel;
    [SerializeField] GameObject ErrorMessage;

    GameObject spawnedBlankPanel;

    void OnEnable()
    {
        InvokeRepeating("OnUpdate", 0.0f, 0.5f);
    }

    void OnDisable()
    {
        if (IsInvoking("OnUpdate"))
            CancelInvoke("OnUpdate");
    }

    void OnUpdate()
    {
        PrestigeButtonText.text = GameState.Stage.stage >= StageData.MIN_PRESTIGE_STAGE ? "Cash Out" : "Locked Stage " + StageData.MIN_PRESTIGE_STAGE.ToString();

        PrestigePointText.text = Utils.Format.FormatNumber(GameState.Player.prestigePoints) + " (<color=orange>+" 
            + Utils.Format.FormatNumber(Formulas.CalcPrestigePoints(GameState.Stage.stage)) + "</color>)";
    }

    // === Button Callbacks ===

    public void OnPrestige()
    {
        if (GameState.Stage.stage < StageData.MIN_PRESTIGE_STAGE)
            return;

        JSONNode node = Utils.Json.GetDeviceNode();

        node.Add("prestigeStage", GameState.Stage.stage);

        spawnedBlankPanel = Utils.UI.Instantiate(BlankPanel, Vector3.zero);

        Server.Prestige(this, OnPrestigeCallback, node);
    }

    public void OnPrestigeCallback(long code, string compressedJson)
    {
        if (code == 200)
        {
            JSONNode node = JSON.Parse(Utils.GZip.Unzip(System.Convert.FromBase64String(compressedJson)));

            PrestigeManager.StartPrestige(node);
        }

        else
        {
            Utils.UI.ShowError(ErrorMessage, "Server Connection", "A server connection is required to cash out!");

            Destroy(spawnedBlankPanel);
        }
    }
}
