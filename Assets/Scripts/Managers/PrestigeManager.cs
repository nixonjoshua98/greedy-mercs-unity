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

        Instance.spawnedBlankPanel = Utils.UI.Instantiate(Instance.BlankPanel, Vector3.zero);

        Server.Prestige(Instance, Instance.OnPrestigeCallback, node);
    }

    void OnPrestigeCallback(long code, string compressed)
    {
        if (code == 200)
        {
            DataManager.IsPaused = true;

            SquadManager.ToggleAttacks(false);

            GameState.Restore(Utils.Json.Decode(compressed));

            Utils.File.WriteJson(DataManager.LOCAL_FILENAME, GameState.ToJson());

            StartCoroutine(PrestigeAnimation());
        }

        else
        {
            Utils.UI.ShowError(ErrorMessage, "Server Connection", "A server connection is required to cash out!");

            Destroy(spawnedBlankPanel);
        }
    }

    IEnumerator PrestigeAnimation()
    {
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadSceneAsync("GameScene");
    }
}