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
                    return BonusType.MELEE_DAMAGE;

                case AttackType.RANGED:
                    return BonusType.RANGED_DAMAGE;

                default:
                    return BonusType.MELEE_DAMAGE;
            }
        }
    }
}