using System;

using SimpleJSON;

namespace GM
{
    public static class GameState
    {
        public static UpgradesContainer Upgrades;
        public static CharacterContainer Characters;

        public static void Restore(JSONNode node)
        {
            Upgrades    = new UpgradesContainer(node);
            Characters  = new CharacterContainer(node);
        }

        public static void Prestige()
        {
            Upgrades.Clear();
            Characters.Clear();
        }
    }
}