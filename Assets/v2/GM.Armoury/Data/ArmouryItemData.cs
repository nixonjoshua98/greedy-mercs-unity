using UnityEngine;

namespace GM.Armoury.Data
{
    public struct ArmouryItemData
    {
        Models.ArmouryItemGameDataModel Game;
        Models.ArmouryItemUserDataModel User;

        public ArmouryItemData(Models.ArmouryItemGameDataModel game, Models.ArmouryItemUserDataModel user)
        {
            Game = game;
            User = user;
        }

        public ItemTierDisplayConfig Config => ArmouryUtils.GetDisplayConfig(Tier);

        public Sprite Icon => Game.Icon;
        public string ItemName => Game.Name;
        public int MergeCost => Game.BaseMergeCost;
        public int CurrentMergeLevel => User.MergeLevel;

        /// <summary>
        /// </summary>
        public int NumOwned => User.NumOwned;

        /// <summary>
        /// </summary>
        public int CurrentLevel => User.Level;

        /// <summary>
        /// Forward the value
        /// </summary>
        public int Tier => Game.Tier;

        /// <summary>
        /// Check if the user can evolve the item (A more detailed check is on the server)
        /// </summary>
        public bool CanMerge => User.NumOwned >= Game.BaseMergeCost && User.MergeLevel < Game.MaxMergeLevel;

        /// <summary>
        /// Current weapon damage based on the level and evolve level
        /// </summary>
        public double WeaponDamage => WeaponDamageFor(User.Level, User.MergeLevel);

        /// <summary>
        /// Weapon damage for the supplied values
        /// </summary>
        public double WeaponDamageFor(int level, int starLevel) => GM.Common.GameFormulas.ArmouryItemDamage(level, starLevel, Game.BaseDamage);
    }
}