using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using SimpleJSON;

public class PrestigeManager : MonoBehaviour
{
    static PrestigeManager Instance = null;

    [SerializeField] GameObject BlankPanel;
    [SerializeField] GameObject ErrorMessage;

    GameObject spawnedBlankPanel;

    public void Awake()
    {
        Instance = this;
    }

    public static void StartPrestige()
    {
        JSONNode node = Utils.Json.GetDeviceNode();

        node.Add("prestigeStage", GameState.Stage.stage);

        SquadManager.ToggleAttacking(false);

        Instance.spawnedBlankPanel = Utils.UI.Instantiate(Instance.BlankPanel, Vector3.zero);

        Server.Prestige(Instance, Instance.OnPrestigeCallback, node);
    }

    void OnPrestigeCallback(long code, string compressed)
    {
        if (code == 200)
        {
            DataManager.IsPaused = true;

            Utils.File.WriteJson(DataManager.LOCAL_FILENAME, Utils.Json.Decode(compressed));

            StartCoroutine(PrestigeAnimation());
        }

        else
        {
            Utils.UI.ShowError(ErrorMessage, "Server Connection", "A server connection is required to cash out!");

            SquadManager.ToggleAttacking(true);

            Destroy(spawnedBlankPanel);
        }
    }

    IEnumerator PrestigeAnimation()
    {
        yield return SquadManager.MoveOut(1.0f);

        bool _ = Utils.File.ReadJson(DataManager.LOCAL_FILENAME, out JSONNode node);

        GameState.Restore(node);

        SceneManager.LoadSceneAsync("GameScene");
    }
}