namespace GM.Artefacts.UI
{
    public abstract class ArtefactUIObject : Core.GMMonoBehaviour
    {
        protected int AssignedArtefactId = -1;

        public virtual void AssignArtefact(int artefactId)
        {
            AssignedArtefactId = artefactId;
        }

        protected Data.ArtefactData AssignedArtefact { get => App.Data.Arts.GetArtefact(AssignedArtefactId); }
    }
}
