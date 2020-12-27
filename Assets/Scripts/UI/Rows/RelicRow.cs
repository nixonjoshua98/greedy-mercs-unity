using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class RelicRow : MonoBehaviour
{
    [SerializeField] RelicID relicId;

    [Header("Components")]
    public Text NameText;
    public Text DescriptionText;

    void UpdateRow()
    {
        RelicStaticData data = StaticData.GetRelic(relicId);

        NameText.text = data.name;

        DescriptionText.text = data.description
            .Replace("{relicEffect}", "<color=orange>" + Formulas.CalculateRelicEffect(relicId) + "x</color>");
    }

    public bool TryUpdate()
    {
        if (GameState.Relics.TryGetRelic(relicId, out var _))
        {
            UpdateRow();

            return true;
        }

        return false;
    }
}
