namespace GM.Armoury.Data
{
    public struct FullArmouryItemData
    {
        public ArmouryItemGameData Game;
        public ArmouryItemState User;

        public FullArmouryItemData(ArmouryItemGameData game, ArmouryItemState user)
        {
            Game = game;
            User = user;
        }

        public bool ReadyToEvolve => User.NumOwned >= (Game.EvoLevelCost + 1) && User.EvoLevel < Game.MaxEvolveLevel;

        public int UpgradeCost()
        {
            return 5 + (Game.Tier + 1) + User.Level;
        }

        public double WeaponDamage => WeaponDamageFor(User.Level, User.EvoLevel);

        public double WeaponDamageFor(int level, int evoLevel)
        {
            double val = ((evoLevel + 1) * ((Game.BaseDamageMultiplier) - 1) * level) + 1;

            return val > 1 ? val : 0;
        }
    }
}
