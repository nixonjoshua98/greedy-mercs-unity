using System;

using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsPanel : MonoBehaviour
{
    [SerializeField] Text BonusTypeText;
    [SerializeField] Text BonusTypeValueText;

    IEnumerator Start()
    {
        foreach (BonusType bonusType in Enum.GetValues(typeof(BonusType)))
        {
            BigDouble val;

            switch (bonusType)
            {
                case BonusType.CRIT_CHANCE:
                    val = StatsCache.GetCritChance();
                    break;

                case BonusType.CRIT_DAMAGE:
                    val = StatsCache.GetCritDamage();
                    break;

                case BonusType.HERO_TAP_DAMAGE_ADD:
                    continue;

                default:
                    val = StatsCache.MultiplyBonuses(bonusType);
                    break;            
            }

            BonusTypeText.text      += Utils.Generic.BonusToString(bonusType) + "\n";
            BonusTypeValueText.text += Utils.Format.FormatNumber(val * 100) + "%" + "\n";

            yield return new WaitForFixedUpdate();
        }
    }

    public void OnClose()
    {
        Destroy(gameObject);
    }
}
