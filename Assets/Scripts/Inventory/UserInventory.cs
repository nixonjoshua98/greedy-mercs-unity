using UnityEngine.Events;

namespace SRC.Inventory.Data
{
    public class UserInventory : SRC.Core.GMClass
    {
        public double PrestigePoints;

        public int Diamonds;
        public long BountyPoints { get; set; }
        public long ArmouryPoints { get; set; }
        public BigDouble Gold { get => App.LocalStateFile.Gold; set => App.LocalStateFile.Gold = value; }

        /* Events */
        public UnityEvent<long> BountyPointsChanged = new();
        public UnityEvent<BigDouble> GoldChanged = new();
        public UnityEvent<double> PrestigePointsChanged = new();
        public UnityEvent<long> ArmouryPointsChanged = new();

        public void Set(UserCurrencies currencies)
        {
            UpdateCurrencies(currencies);
        }

        public void UpdateCurrencies(UserCurrencies model)
        {
            BountyPoints = model.BountyPoints;
            ArmouryPoints = model.ArmouryPoints;
            PrestigePoints = model.PrestigePoints;
        }
    }
}
