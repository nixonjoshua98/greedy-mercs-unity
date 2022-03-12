using GM.Artefacts.Models;
using UnityEngine;
using BonusType = GM.Common.Enums.BonusType;

namespace GM.Artefacts.Data
{
    public class AggregatedArtefactData : Core.GMClass
    {
        public readonly int Id;

        ArtefactGameDataModel Game => App.DataContainers.Artefacts.GetGameArtefact(Id);
        ArtefactUserDataModel User => App.DataContainers.Artefacts.GetUserArtefact(Id);

        public AggregatedArtefactData(int artefactId)
        {
            Id = artefactId;
        }

        // Can be used to pre-upgrade an artefact before the server replies
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
        public BigDouble Effect => App.GMCache.ArtefactEffect(this);
        public bool IsMaxLevel => User.Level >= Game.MaxLevel;
    }
}
