namespace GM.Artefacts.UI
{
    public abstract class ArtefactUIObject : GM.UI.SlotObject
    {
        int AssignedArtefactId = -1;

        public virtual void AssignArtefact(int artefactId)
        {
            AssignedArtefactId = artefactId;

            OnAssigned();
        }

        protected virtual void OnAssigned() { }

        protected Data.AggregatedArtefactData AssignedArtefact => App.GMData.Artefacts.GetArtefact(AssignedArtefactId);

        protected string GetBonusText() => Format.Bonus(AssignedArtefact.Bonus, AssignedArtefact.Effect, "orange");
    }
}
