using UnityEngine;
using GM.Common.Enums;

namespace GM.Armoury.Data
{
    public class ArmouryItemData : Core.GMClass
    {
        Models.ArmouryItemGameDataModel Game;
        Models.ArmouryItemUserDataModel User;

        public ArmouryItemData(Models.ArmouryItemGameDataModel game, Models.ArmouryItemUserDataModel user)
        {
            Game = game;
            User = user;
        }

        public int Id => Game.Id;

        public Sprite Icon => Game.Icon;
        public string ItemName => Game.Name;
        public BonusType BonusType => Game.BonusType;
        public float BaseEffect => Game.BaseEffect;
        public float LevelEffect => Game.LevelEffect;

        public int NumOwned => User.NumOwned;
        public int CurrentLevel => User.Level;

        public double BonusValue => App.Cache.ArmouryItemBonusValue(this);
        public int UpgradeCost() => App.Cache.ArmouryItemUpgradeCost(this);
    }
}