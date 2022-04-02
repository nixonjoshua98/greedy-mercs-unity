namespace GM.Bounties.UI
{
    public abstract class UnlockedBountyUIObject : Core.GMMonoBehaviour
    {
        int AssignedBountyId = -1;

        public virtual void Assign(int bountyId)
        {
            AssignedBountyId = bountyId;

            OnAssigned();
        }

        protected abstract void OnAssigned();

        public Models.AggregatedBounty AssignedBounty { get => App.Bounties.GetUnlockedBounty(AssignedBountyId); }
    }
}
