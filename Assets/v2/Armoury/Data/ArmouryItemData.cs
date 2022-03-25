using GM.Common.Enums;
using UnityEngine;

namespace GM.Armoury.Data
{
    public class ArmouryItemData : Core.GMClass
    {
        Models.ArmouryItem Game;
        Models.ArmouryItemUserDataModel User;

        public ArmouryItemData(Models.ArmouryItem game, Models.ArmouryItemUserDataModel user)
        {
            Game = game;
            User = user;
        }

        public int Id => Game.ID;

        public Sprite Icon => Game.Icon;
        public string ItemName => Game.Name;
        public BonusType BonusType => Game.BonusType;
        public float BaseEffect => Game.BaseEffect;
        public float LevelEffect => Game.LevelEffect;

        public int NumOwned => User.NumOwned;
        public int CurrentLevel => User.Level;

        public double BonusValue => App.GMCache.ArmouryItemBonusValue(this);
        public int UpgradeCost() => App.GMCache.ArmouryItemUpgradeCost(this);
    }
}