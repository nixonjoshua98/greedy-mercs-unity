using UnityEngine;
using UnityEngine.SceneManagement;

using SimpleJSON;

public class PrestigeManager : MonoBehaviour
{
    PrestigeManager Instance = null;

    public void Awake()
    {
        Instance = this;
    }

    public static void StartPrestige(JSONNode node)
    {
        DataManager.IsPaused = true;

        SquadManager.ToggleAttacks(false);

        GameState.RestoreDefaults();

        DataManager.Save();

        SceneManager.LoadSceneAsync(0);
    }
}