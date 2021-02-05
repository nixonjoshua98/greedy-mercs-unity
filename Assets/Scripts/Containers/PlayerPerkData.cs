using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

namespace GreedyMercs.Perks.Data
{
    public enum PerkID
    {
        ENERGY_POTION   = 0,
        DAMAGE_BOOST    = 1,
        ENEMY_COOLDOWN  = 2,
    }

    class PerkState
    {
        public DateTime ActivatedAt;

        public int SecondsSinceActivated { get { return Mathf.CeilToInt((float)(DateTime.UtcNow - ActivatedAt).TotalSeconds); } }
        public int DurationRemaining { get { return (60 * 60 * 6) - SecondsSinceActivated; } }
        public bool IsActive { get { return SecondsSinceActivated >= 0 && DurationRemaining > 0; } }
    }


    public class PlayerPerkData
    {
        Dictionary<PerkID, PerkState> perks;

        public PlayerPerkData(JSONNode node)
        {
            Update(node);
        }

        public void Update(JSONNode node)
        {
            perks = new Dictionary<PerkID, PerkState>();

            foreach (string key in node.Keys)
            {
                PerkID perk = (PerkID)int.Parse(key);

                DateTime activatedTime = node[key]["ActivatedAt"].AsLong.ToUnixDatetime();

                perks.Add(perk, new PerkState() { ActivatedAt = activatedTime });
            }
        }

        public JSONNode ToJson()
        {
            JSONNode node = new JSONObject();

            foreach (var entry in perks)
            {
                JSONNode perkNode = new JSONObject();

                perkNode.Add("ActivatedAt", entry.Value.ActivatedAt.ToUnixMilliseconds());

                node.Add(((int)entry.Key).ToString(), perkNode);
            }

            return node;
        }

        public Dictionary<BonusType, double> CalcBonuses()
        {
            Dictionary<BonusType, double> bonuses = new Dictionary<BonusType, double>();

            if (IsPerkActive(PerkID.DAMAGE_BOOST))
                bonuses[BonusType.MERC_DAMAGE] = 5.0;

            if (IsPerkActive(PerkID.ENERGY_POTION))
                bonuses[BonusType.ENERGY_INCOME] = 2.0;

            return bonuses;
        }

        public int PerkDurationRemaining(PerkID perk)
        {
            return perks[perk].DurationRemaining;
        }

        public bool IsPerkActive(PerkID perk)
        {
            if (!perks.ContainsKey(perk))
                return false;

            return perks[perk].IsActive;
        }

        public void ActivatePerk(PerkID perk)
        {
            perks[perk] = new PerkState() { ActivatedAt = DateTime.UtcNow };
        }
    }
}