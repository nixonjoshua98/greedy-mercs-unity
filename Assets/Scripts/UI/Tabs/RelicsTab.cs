
using UnityEngine;
using UnityEngine.UI;

using SimpleJSON;

public class RelicsTab : MonoBehaviour
{
    [SerializeField] BuyAmountController buyAmount;

    [SerializeField] Text PrestigePointText;
    [SerializeField] Text PrestigeButtonText;
    [SerializeField] Text RelicCostText;

    [Space]

    [SerializeField] GameObject BuyRelicsButton;

    [Space]

    [SerializeField] GameObject BlankPanel;
    [SerializeField] GameObject ErrorMessage;

    GameObject spawnedBlankPanel;

    void Start()
    {
        if (GameState.NumRelicsOwned == StaticData.numRelics)
            Destroy(BuyRelicsButton);
    }

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

        if (GameState.NumRelicsOwned < StaticData.numRelics)
            RelicCostText.text = Utils.Format.FormatNumber(Formulas.CalcNextRelicCost(GameState.NumRelicsOwned));
    }

    // === Button Callbacks ===

    public void OnPrestige()
    {
        if (GameState.Stage.stage < StageData.MIN_PRESTIGE_STAGE)
            return;

        PrestigeManager.StartPrestige();
    }

    public void OnBuyRelic()
    {
        spawnedBlankPanel = Utils.UI.Instantiate(BlankPanel, Vector3.zero);

        Server.BuyRelic(this, OnBuyRelicCallback, Utils.Json.GetDeviceNode());
    }

    public void OnBuyRelicCallback(long code, string data)
    {
        if (code == 200)
        {
            JSONNode node = Utils.Json.Decompress(data);
        }

        else
        {
            Utils.UI.ShowError(ErrorMessage, "Relic", "A connection to the server is required");
        }

        Destroy(spawnedBlankPanel);
    }
}
