namespace GM.Artefacts.UI
{
    public abstract class ArtefactUIObject : GM.UI.SlotObject
    {
        private int AssignedArtefactId = -1;

        public virtual void AssignArtefact(int artefactId)
        {
            AssignedArtefactId = artefactId;

            OnAssigned();
        }

        protected virtual void OnAssigned() { }

        protected Data.AggregatedArtefactData AssignedArtefact => App.Artefacts.GetArtefact(AssignedArtefactId);

        protected string GetBonusText()
        {
            return Format.Bonus(AssignedArtefact.Bonus, AssignedArtefact.Effect, "orange");
        }
    }
}
