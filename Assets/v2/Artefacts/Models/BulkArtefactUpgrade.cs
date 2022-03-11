namespace GM.Artefacts.Models
{
    public class BulkArtefactUpgrade
    {
        public int ArtefactID;
        public int UpgradeLevels;

        public BulkArtefactUpgrade(int artefact, int levels)
        {
            ArtefactID = artefact;
            UpgradeLevels = levels;
        }
    }
}
