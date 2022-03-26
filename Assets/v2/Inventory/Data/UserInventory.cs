namespace GM.Inventory.Data
{
    public class UserInventory
    {
        public double PrestigePoints;

        public long BountyPoints { get; private set; }
        public long ArmouryPoints { get; private set; }

        public BigDouble Gold;

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
