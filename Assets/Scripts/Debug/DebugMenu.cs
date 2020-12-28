using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    public void OnGoldButton()
    {
        GameState.Player.gold = BigDouble.Parse("1.0e500");
    }

    public void OnStageAdvance()
    {
        GameState.Stage.stage += 100;
    }
}
