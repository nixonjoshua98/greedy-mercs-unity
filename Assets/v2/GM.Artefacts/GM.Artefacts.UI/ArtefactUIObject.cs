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

        protected Data.ArtefactData AssignedArtefact => App.Data.Artefacts.GetArtefact(AssignedArtefactId);
    }
}
