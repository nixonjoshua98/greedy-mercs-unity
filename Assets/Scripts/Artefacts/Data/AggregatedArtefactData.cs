using SRC.Common.Enums;
using SRC.ScriptableObjects;
using System;
using UnityEngine;

namespace SRC.Artefacts.Data
{
    public partial class AggregatedArtefactData
    {
        public readonly int ArtefactID;

        public Func<Artefact> GetArtefact;
        public Func<UserArtefact> GetUserArtefact;

        private Artefact DataFile => GetArtefact();
        private UserArtefact User => GetUserArtefact();

        public AggregatedArtefactData(int artefactId, Func<Artefact> getArtefact, Func<UserArtefact> getUserArtefact)
        {
            ArtefactID = artefactId;

            GetArtefact = getArtefact;
            GetUserArtefact = getUserArtefact;
        }
    }

    public partial class AggregatedArtefactData : Core.GMClass
    {
        public int LocalLevelChange = 0;

        public int CurrentLevel => User.Level + LocalLevelChange;
        public int MaxLevel => DataFile.MaxLevel;
        public float LevelEffect => DataFile.LevelEffect;
        public string Name => DataFile.Name;
        public Sprite Icon => DataFile.Icon;
        public ItemGradeData Grade => App.Local.GetItemGrade(DataFile.GradeID);
        public BonusType BonusType => DataFile.BonusType;
        public double BaseEffect => DataFile.BaseEffect;
        public float CostExpo => DataFile.CostExpo;
        public float CostCoeff => DataFile.CostCoeff;
        public double BonusValue => App.Bonuses.ArtefactBonus(this);
        public bool IsMaxLevel => CurrentLevel >= DataFile.MaxLevel;
        public double UpgradeCost(int levels) => App.Bonuses.ArtefactUpgradeCost(this, levels);
    }
}
