using UnityEngine;

namespace GM.Armoury
{
    public struct ItemTierDisplayConfig
    {
        public Color Colour;
        public string DisplayText;
    }

    public static class ArmouryUtils
    {
        public static ItemTierDisplayConfig GetDisplayConfig(int tier)
        {
            return tier switch
            {
                0 => new ItemTierDisplayConfig() { Colour = new Color(0.5f, 32.0f/255, 32.0f/255), DisplayText = "D" }, // Grey
                1 => new ItemTierDisplayConfig() { Colour = new Color(32.0f/255, 1.0f, 64.0f/255), DisplayText = "C" }, // Green
                2 => new ItemTierDisplayConfig() { Colour = new Color(32.0f/255, 200.0f/255, 1.0f), DisplayText = "B" }, // Blue
                _ => new ItemTierDisplayConfig() { Colour = Color.white, DisplayText = tier.ToString() }, // Default
            };
        }
    }
}
