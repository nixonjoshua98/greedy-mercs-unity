using SRC.Common;
using SRC.Common.Enums;
using SRC.ScriptableObjects;
using UnityEngine;

namespace SRC.Artefacts.Data
{
    public class AggregatedArtefactData : Core.GMClass
    {
        public readonly int Id;

        private Artefact Game => App.Artefacts.GetGameArtefact(Id);

        private ArtefactUserData User => App.Artefacts.GetUserArtefact(Id);

        public AggregatedArtefactData(int artefactId)
        {
            Id = artefactId;
        }

        public int LocalLevelChange = 0;

        public int CurrentLevel => User.Level + LocalLevelChange;
        public int MaxLevel => Game.MaxLevel;
        public float LevelEffect => Game.LevelEffect;
        public string Name => Game.Name;
        public Sprite Icon => Game.Icon;
        public ItemGradeData Grade => App.Local.GetItemGrade(Game.GradeID);
        public BonusType BonusType => Game.Bonus;
        public double BaseEffect => Game.BaseEffect;
        public float CostExpo => Game.CostExpo;
        public float CostCoeff => Game.CostCoeff;
        public double BonusValue => App.Bonuses.ArtefactBonus(this);
        public double BonusValueBase => GameFormulas.ArtefactBonus(CurrentLevel, BaseEffect, LevelEffect);
        public bool IsMaxLevel => CurrentLevel >= Game.MaxLevel;
        public double UpgradeCost(int levels) => App.Bonuses.ArtefactUpgradeCost(this, levels);
    }
}
