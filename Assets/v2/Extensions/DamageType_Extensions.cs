using GM.Common;
using GM.Common.Enums;
using UnityEngine;

namespace GM
{
    public static class DamageType_Extensions
    {
        public static Color TextColor(this DamageType dmg)
        {
            return dmg switch
            {
                DamageType.Normal => Constants.Colors.SoftRed,
                DamageType.EnergyOvercharge => Constants.Colors.SoftBlue,
                DamageType.CriticalHit => Constants.Colors.Orange,
                _ => Constants.Colors.SoftRed
            };
        }
    }
}
