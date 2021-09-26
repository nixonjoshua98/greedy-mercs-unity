namespace GM.Bounties.Data
{
    public class UserBountyState
    {
        public int ID;

        public bool IsActive;

        public UserBountyState(int bounty)
        {
            ID = bounty;
        }
    }
}
