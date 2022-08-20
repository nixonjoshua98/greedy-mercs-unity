using SRC.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SRC.Core
{
    public partial class ResolvedBonusDictionary : GMClass
    {
        public static double GetBonus(BonusType bonusType) => Dictionary.Get(bonusType, bonusType.DefaultValue());
    }

    public partial class ResolvedBonusDictionary : GMClass
    {
        static Dictionary<BonusType, double> _resolvedBonuses;
        static DateTime _lastUpdated = DateTime.MinValue;


        static Dictionary<BonusType, double> Artefacts
            => CreateBonusDict(App.Artefacts.UnlockedArtefacts
                .Select(x => new KeyValuePair<BonusType, double>(x.BonusType, x.BonusValue))
                .ToList());

        static Dictionary<BonusType, double> Armoury
            => CreateBonusDict(App.Armoury.UserItems
                .Select(x => new KeyValuePair<BonusType, double>(x.BonusType, x.BonusValue))
                .ToList());

        static Dictionary<BonusType, double> Bounties
            => CreateBonusDict(App.Bounties.UnlockedBounties
                .Select(x => new KeyValuePair<BonusType, double>(x.BonusType, x.BonusValue))
                .ToList());

        static Dictionary<BonusType, double> MercPassives
            => CreateBonusDict(App.Mercs.UnlockedMercs
                .SelectMany(merc => merc.UnlockedPassives)
                .Select(x => new KeyValuePair<BonusType, double>(x.BonusType, x.BonusValue))
                .ToList());

        static Dictionary<BonusType, double> Dictionary => CreateResolvedBonusDict();

        static Dictionary<BonusType, double> CreateResolvedBonusDict()
        {
            return CombineBonusDicts(Artefacts, Armoury, Bounties, MercPassives);
        }

        static Dictionary<BonusType, double> CombineBonusDicts(params Dictionary<BonusType, double>[] bonusDicts)
        {
            Dictionary<BonusType, double> result = new();

            foreach (var dict in bonusDicts)
            {
                foreach (var pair in dict)
                {
                    UpdateBonusValue(result, pair.Key, pair.Value);
                }
            }

            return result;
        }

        static Dictionary<BonusType, double> CreateBonusDict(List<KeyValuePair<BonusType, double>> ls)
        {
            Dictionary<BonusType, double> result = new();

            ls.ForEach(pair => UpdateBonusValue(result, pair.Key, pair.Value));

            return result;
        }

        static void UpdateBonusValue(Dictionary<BonusType, double> dict, BonusType bonusType, double newValue)
        {
            if (!dict.TryGetValue(bonusType, out double totalValue))
            {
                dict[bonusType] = newValue;
            }
            else
            {
                dict[bonusType] = UpdateBonusValue(bonusType, totalValue, newValue);
            }
        }

        static double UpdateBonusValue(BonusType bonusType, double currentValue, double newValue)
        {
            return bonusType switch
            {
                BonusType.FLAT_CRIT_CHANCE => currentValue + newValue,
                _ => currentValue * newValue
            };
        }
    }
}
