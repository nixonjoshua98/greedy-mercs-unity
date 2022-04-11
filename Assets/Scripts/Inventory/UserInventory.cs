using GM.Common.Enums;
using UnityEngine.Events;

namespace GM.Inventory.Data
{
    public class UserInventory
    {
        public double PrestigePoints;

        public int Diamonds;
        public long BountyPoints { get; set; }
        public long ArmouryPoints { get; set; }

        public BigDouble Gold;

        public UnityEvent<long> BountyPointsChanged = new();
        public UnityEvent<BigDouble> GoldChanged = new();
        public UnityEvent<double> PrestigePointsChanged = new();
        public UnityEvent<long> ArmouryPointsChanged = new();

        public void Set(UserCurrencies currencies)
        {
            Gold = BigDouble.HighValue;

            UpdateCurrencies(currencies);
        }

        public string GetCurrencyString(CurrencyType type)
        {
            return Format.Number(type switch
            {
                CurrencyType.ARMOURY_POINTS => ArmouryPoints,
                CurrencyType.BOUNTY_POINTS => BountyPoints,
                CurrencyType.DIAMONDS => Diamonds,
                CurrencyType.GOLD => Gold,
                CurrencyType.PRESTIGE_POINTS => PrestigePoints
            });
        }

        public void UpdateCurrencies(UserCurrencies model)
        {
            BountyPoints = model.BountyPoints;
            ArmouryPoints = model.ArmouryPoints;
            PrestigePoints = model.PrestigePoints;
        }

        public void DeleteLocalStateData()
        {
            Gold = 0;
        }
    }
}
