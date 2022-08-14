using GM.Mercs.Data;

namespace GM.Mercs.UI
{
    public abstract class MercUIObject : GM.Core.GMMonoBehaviour
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
