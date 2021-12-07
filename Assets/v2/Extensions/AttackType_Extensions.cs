using GM.Common.Enums;

namespace GM
{
    public static class AttackType_Extensions
    {
        public static BonusType Bonus(this AttackType val)
        {
            switch (val)
            {
                case AttackType.MELEE:
                    return BonusType.MULTIPLY_MELEE_DMG;

                case AttackType.RANGED:
                    return BonusType.MULTIPLY_MELEE_DMG;

                default:
                    return BonusType.MULTIPLY_MELEE_DMG;
            }
        }
    }
}