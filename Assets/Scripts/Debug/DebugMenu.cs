using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugMenu : MonoBehaviour
{
    public void OnGoldButton()
    {
        GameState.Player.gold = BigDouble.Parse("1.0e5000");
    }

    public void OnStageAdvance()
    {
        GameState.Stage.stage += 1_000;
    }

    public void OnResetRelics()
    {
        Server.ResetRelics(this, ResetRelicsCallback);
    }

    void ResetRelicsCallback(long code, string _)
    {
        SceneManager.LoadSceneAsync("LoginScene");
    }
}
