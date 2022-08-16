using SRC.Mercs.Data;

namespace SRC.Mercs.UI
{
    public abstract class MercUIObject : SRC.Core.GMMonoBehaviour
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
