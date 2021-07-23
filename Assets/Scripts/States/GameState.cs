using System;

using SimpleJSON;

namespace GM
{
    public static class GameState
    {
        public static PlayerState Player;

        public static UpgradesContainer Upgrades;
        public static CharacterContainer Characters;

        public static void Restore(JSONNode node)
        {
            Player = new PlayerState();

            Upgrades    = new UpgradesContainer(node);
            Characters  = new CharacterContainer(node);
        }

        public static void Prestige()
        {
            Player.Reset();

            Upgrades.Clear();
            Characters.Clear();
        }
    }
}