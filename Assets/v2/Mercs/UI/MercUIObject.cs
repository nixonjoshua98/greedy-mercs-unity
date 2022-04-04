using GM.UI;
using MercID = GM.Common.Enums.MercID;

namespace GM.Mercs.UI
{
    public abstract class MercUIObject : SlotObject
    {
        private MercID AssignedId;

        public void Assign(MercID id)
        {
            AssignedId = id;

            OnAssigned();
        }

        protected virtual void OnAssigned() { }

        public Data.AggregatedMercData AssignedMerc => App.Mercs.GetMerc(AssignedId);
    }
}
