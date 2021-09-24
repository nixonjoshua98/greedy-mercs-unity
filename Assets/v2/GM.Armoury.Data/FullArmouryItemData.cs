namespace GM.Armoury.Data
{
    public struct FullArmouryItemData
    {
        public ArmouryItemData Game;
        public ArmouryItemState User;

        public FullArmouryItemData(ArmouryItemData game, ArmouryItemState user)
        {
            Game = game;
            User = user;
        }

        public int UpgradeCost()
        {
            return 5 + (Game.Tier + 1) + User.Level;
        }

        public bool ReadyToEvolve => User.NumOwned >= (Game.EvoLevelCost + 1) && User.EvoLevel < Game.MaxEvolveLevel;
    }
}
