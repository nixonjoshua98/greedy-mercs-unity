
using UnityEngine;
using UnityEngine.UI;

public class RelicsTab : MonoBehaviour
{
    [SerializeField] Text PrestigePointText;
    [SerializeField] Text PrestigeButtonText;

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
        PrestigePointText.text = Utils.Format.DoubleToString(GameState.player.prestigePoints);

        PrestigeButtonText.text = GameState.stage.stage >= StageData.MIN_PRESTIGE_STAGE ? "Prestige" : "Locked Stage " + StageData.MIN_PRESTIGE_STAGE.ToString();
    }
}
