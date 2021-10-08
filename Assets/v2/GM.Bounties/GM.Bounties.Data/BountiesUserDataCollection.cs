using System;
using System.Collections.Generic;

namespace GM.Bounties.Data
{
    public class BountiesUserDataCollection
    {
        public List<Models.SingleBountyUserDataModel> States = new List<Models.SingleBountyUserDataModel>();

        public DateTime LastClaimTime;

        public BountiesUserDataCollection(Models.CompleteBountyDataModel data)
        {
            Update(data);
        }

        public void Update(Models.CompleteBountyDataModel data)
        {
            LastClaimTime = data.LastClaimTime;
            States = data.Bounties;
        }

        public void Update(List<Models.SingleBountyUserDataModel> bounties)
        {
            States = bounties;
        }
    }
}