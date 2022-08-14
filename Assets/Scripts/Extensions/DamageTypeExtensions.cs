using GM.Common;
using GM.Common.Enums;
using UnityEngine;

namespace GM
{
    public static class DamageTypeExtensions
    {
        public static Color TextColor(this DamageType dmg)
        {
            return dmg switch
            {
                DamageType.Normal => Constants.Colors.SoftRed,
                DamageType.CriticalHit => Constants.Colors.Yellow,
                _ => Constants.Colors.SoftRed
            };
        }
    }
}
