
using UnityEngine;
using UnityEngine.UI;

public class RelicsTab : MonoBehaviour
{
    [SerializeField] Text PrestigePointText;
    [SerializeField] Text PrestigeButtonText;

    void OnEnable()
    {
        InvokeRepeating("OnUpdate", 0.0f, 0.5f);

        PrestigePointText.text = Utils.Format.FormatNumber(GameState.Player.prestigePoints);
    }

    void OnDisable()
    {
        if (IsInvoking("OnUpdate"))
            CancelInvoke("OnUpdate");
    }

    void OnUpdate()
    {
        PrestigeButtonText.text = GameState.Stage.stage >= StageData.MIN_PRESTIGE_STAGE ? "Prestige" : "Locked Stage " + StageData.MIN_PRESTIGE_STAGE.ToString();
    }

    // === Button Callbacks ===

    public void OnPrestige()
    {
        if (GameState.Stage.stage < StageData.MIN_PRESTIGE_STAGE)
            return;
    }
}
