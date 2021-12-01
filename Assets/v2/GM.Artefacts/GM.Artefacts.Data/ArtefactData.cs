using GM.Artefacts.Models;
using UnityEngine;
using BonusType = GM.Common.Enums.BonusType;

namespace GM.Artefacts.Data
{
    public class ArtefactData
    {
        ArtefactGameDataModel Game;
        ArtefactUserDataModel User;

        public ArtefactData(ArtefactGameDataModel values, ArtefactUserDataModel state)
        {
            Game = values;
            User = state;
        }

        public int Id => Game.Id;
        public int CurrentLevel => User.Level;
        public int MaxLevel => Game.MaxLevel;
        public string Name => Game.Name;
        public Sprite Icon => Game.Icon;
        public BonusType Bonus => Game.Bonus;
        public float CostExpo => Game.CostExpo;
        public float CostCoeff => Game.CostCoeff;
        public double BaseEffect => GM.Common.GameFormulas.BaseArtefactEffect(User.Level, Game.BaseEffect, Game.LevelEffect);
        public bool IsMaxLevel => User.Level >= Game.MaxLevel;
    }
}
