using SRC.Common.Enums;

namespace SRC
{
    public static class AttackTypeExtensions
    {
        public static BonusType Bonus(this UnitAttackType val)
        {
            switch (val)
            {
                case UnitAttackType.Melee:
                    return BonusType.MULTIPLY_MELEE_DMG;

                case UnitAttackType.Ranged:
                    return BonusType.MULTIPLY_MELEE_DMG;

                default:
                    return BonusType.MULTIPLY_MELEE_DMG;
            }
        }
    }
}