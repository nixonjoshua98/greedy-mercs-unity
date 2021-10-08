namespace GM
{
    public static class GameState
    {
        public static UpgradesContainer Upgrades { get; set; }

        public static void Restore()
        {
            Upgrades = new UpgradesContainer();
        }
    }
}