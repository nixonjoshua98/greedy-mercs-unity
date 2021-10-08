namespace GM.Armoury.Data
{
    public struct FullArmouryItemData
    {
        public Models.ArmouryItemGameDataModel Game;
        public Models.UserArmouryItemModel User;

        public FullArmouryItemData(Models.ArmouryItemGameDataModel game, Models.UserArmouryItemModel user)
        {
            Game = game;
            User = user;
        }

        /// <summary>
        /// Check if the user can evolve the item (A more detailed check is on the server)
        /// </summary>
        public bool ReadyToEvolve => User.NumOwned >= (Game.EvoLevelCost + 1) && User.EvoLevel < Game.MaxEvolveLevel;

        /// <summary>
        /// Calculate the weapon damage based on the current item level
        /// </summary>
        public int UpgradeCost()
        {
            return 5 + (Game.Tier + 1) + User.Level;
        }

        /// <summary>
        /// Current weapon damage based on the level and evolve level
        /// </summary>
        public double WeaponDamage => WeaponDamageFor(User.Level, User.EvoLevel);

        /// <summary>
        /// Weapon damage for the supplied values
        /// </summary>
        public double WeaponDamageFor(int level, int evoLevel) => Formulas.ArmouryItemDamage(level, evoLevel, Game.BaseDamage);
    }
}
