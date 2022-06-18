using GM.Artefacts.Models;
using UnityEngine;
using BonusType = GM.Common.Enums.BonusType;

namespace GM.Artefacts.Data
{
    public class AggregatedArtefactData : Core.GMClass
    {
        public readonly int Id;

        private ArtefactGameDataModel Game => App.Artefacts.GetGameArtefact(Id);

        private ArtefactUserDataModel User => App.Artefacts.GetUserArtefact(Id);

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
        public Sprite IconBackground => Game.IconBackground;
        public BonusType Bonus => Game.Bonus;
        public float BaseEffect => Game.BaseEffect;
        public float CostExpo => Game.CostExpo;
        public float CostCoeff => Game.CostCoeff;
        public BigDouble Effect => App.Values.ArtefactEffect(this);
        public bool IsMaxLevel => CurrentLevel >= Game.MaxLevel;
        public double UpgradeCost(int levels)
        {
            return App.Values.ArtefactUpgradeCost(this, levels);
        }
    }
}
