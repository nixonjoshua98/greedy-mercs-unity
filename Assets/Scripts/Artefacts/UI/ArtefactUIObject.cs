namespace GM.Artefacts.UI
{
    public abstract class ArtefactUIObject : GM.Core.GMMonoBehaviour
    {
        private int ArtefactID = -1;

        protected Data.AggregatedArtefactData Artefact => App.Artefacts.GetArtefact(ArtefactID);

        public virtual void Intialize(int artefactId)
        {
            ArtefactID = artefactId;

            OnIntialize();
        }

        protected virtual void OnIntialize() { }

        protected string GetBonusText()
        {
            return Format.BonusValue(Artefact.Bonus, Artefact.Effect, "orange");
        }
    }
}
