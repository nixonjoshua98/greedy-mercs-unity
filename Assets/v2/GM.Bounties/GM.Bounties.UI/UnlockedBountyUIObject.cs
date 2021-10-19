namespace GM.Bounties.UI
{
    public class UnlockedBountyUIObject : Core.GMMonoBehaviour
    {
        protected int AssignedBountyId = -1;

        public virtual void Assign(int bountyId)
        {
            AssignedBountyId = bountyId;
        }

        protected Data.UnlockedBountyData AssignedBounty { get => App.Data.Bounties.GetUnlockedBounty(AssignedBountyId); }
    }
}
