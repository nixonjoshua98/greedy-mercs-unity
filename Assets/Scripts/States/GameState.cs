using System;

using SimpleJSON;

namespace GM
{
    public static class GameState
    {
        public static UpgradesContainer Upgrades;

        public static void Restore(JSONNode node)
        {
            Upgrades    = new UpgradesContainer(node);
        }

        public static void Prestige()
        {
            Upgrades.Clear();
        }
    }
}