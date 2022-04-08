using UnityEngine.Events;

namespace GM.Inventory.Data
{
    public class UserInventory
    {
        public double PrestigePoints;

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
