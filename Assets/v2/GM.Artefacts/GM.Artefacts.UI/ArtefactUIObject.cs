namespace GM.Artefacts.UI
{
    public abstract class ArtefactUIObject : Core.GMMonoBehaviour
    {
        protected int AssignedArtefactId = -1;

        protected Data.ArtefactData AssignedArtefact { get => App.Data.Arts.GetArtefact(AssignedArtefactId); }
    }
}