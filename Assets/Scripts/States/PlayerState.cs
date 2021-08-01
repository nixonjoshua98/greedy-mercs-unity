

namespace GM
{
    [System.Serializable]
    public class PlayerState
    {
        public double currentEnergy;

        public BigDouble gold;

        public PlayerState()
        {
            Reset();
        }

        public void Reset()
        {
            gold            = 0;
            currentEnergy   = 0;
        }
    }
}