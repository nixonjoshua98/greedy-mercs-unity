
using UnityEngine;
using UnityEngine.UI;

public class RelicsTab : MonoBehaviour
{
    [SerializeField] BuyAmountController buyAmount;

    [SerializeField] Text PrestigePointText;
    [SerializeField] Text PrestigeButtonText;

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

        PrestigeManager.StartPrestige();
    }
}
