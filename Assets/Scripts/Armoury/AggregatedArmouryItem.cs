using GM.Armoury.Data;
using GM.Common.Enums;
using GM.ScriptableObjects;
using GMCommon.Enums;
using UnityEngine;

namespace GM.Armoury
{
    public class AggregatedArmouryItem : Core.GMClass
    {
        public readonly int ID;

        public Sprite Icon;
        public string Name;
        public BonusType BonusType;
        public float BaseEffect;
        public float LevelEffect;
        public ItemGrade Grade;

        public ItemGradeConfig GradeConfig;

        public AggregatedArmouryItem(int itemId)
        {
            var gameItem = App.Armoury.GetGameItem(itemId);

            ID = itemId;

            Icon = gameItem.Icon;
            Name = gameItem.Name;
            Grade = gameItem.Grade;
            BonusType = gameItem.BonusType;
            BaseEffect = gameItem.BaseEffect;
            LevelEffect = gameItem.LevelEffect;

            GradeConfig = App.LocalGameData.GetItemGradeConfig(Grade);
        }

        UserArmouryItem UserModel
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

        public double BonusValue => App.GMCache.ArmouryItemBonusValue(this);

        public int UpgradeCost()
        {
            return App.GMCache.ArmouryItemUpgradeCost(this);
        }
    }
}