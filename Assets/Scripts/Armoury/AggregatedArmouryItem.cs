using SRC.Armoury.Data;
using SRC.Common.Enums;
using SRC.ScriptableObjects;
using UnityEngine;

namespace SRC.Armoury
{
    public class AggregatedArmouryItem : Core.GMClass
    {
        public readonly int ID;

        public Sprite Icon;
        public string Name;
        public BonusType BonusType;
        public float BaseEffect;
        public float LevelEffect;
        public Rarity Grade;

        public ItemGradeData GradeConfig;

        private ArmouryItem GameItem => App.Armoury.GetGameItem(ID);

        public AggregatedArmouryItem(int itemId)
        {
            ID = itemId;

            Icon = GameItem.Icon;
            Name = GameItem.Name;
            Grade = GameItem.Grade;
            BonusType = GameItem.BonusType;
            BaseEffect = GameItem.BaseEffect;
            LevelEffect = GameItem.LevelEffect;

            GradeConfig = App.Local.GetItemGrade(Grade);
        }

        private UserArmouryItem UserModel
        {
            get
            {
                if (!App.Armoury.TryGetOwnedItem(ID, out UserArmouryItem model))
                    Debug.LogError($"Armoury item '{ID}' not unlocked but attempted to access");

                return model;
            }
        }

        public int NumOwned => UserModel.NumOwned;
        public int CurrentLevel => UserModel.Level;
        public double BonusValue => App.Bonuses.ArmouryItemBonus(this);
        public int UpgradeCost() => App.Bonuses.ArmouryItemUpgradeCost(this);
    }
}