using GM.Artefacts.Models;
using UnityEngine;

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

        // == User == //
        public int CurrentLevel => User.Level;


        // == Game == //
        public int MaxLevel => Game.MaxLevel;
        public string Name => Game.Name;
        public Sprite Icon => Game.Icon;
        public BonusType Bonus => Game.Bonus;


        // == Combined == //
        public float CostExpo => Game.CostExpo;
        public float CostCoeff => Game.CostCoeff;
        public double BaseEffect => Formulas.BaseArtefactEffect(User.Level, Game.BaseEffect, Game.LevelEffect);
        public bool IsMaxLevel => User.Level >= Game.MaxLevel;
        public System.Numerics.BigInteger CostToUpgrade(int levels) => Formulas.ArtefactLevelUpCost(User.Level, levels, Game.CostExpo, Game.CostCoeff);
    }
}
