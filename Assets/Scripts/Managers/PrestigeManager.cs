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

    void OnPrestigeCallback(long code, string data)
    {
        if (code == 200)
        {
            DataManager.IsPaused = true;

            Utils.File.Delete(DataManager.LOCAL_FILENAME);

            SceneManager.LoadSceneAsync(0);
        }

        else
        {
            Utils.UI.ShowError(ErrorMessage, "Server Connection", "A server connection is required to cash out!");

            Destroy(spawnedBlankPanel);
        }
    }
}