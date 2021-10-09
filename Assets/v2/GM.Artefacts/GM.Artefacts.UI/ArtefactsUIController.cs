using GM.Artefacts.Data;
using UnityEngine;
using GameObjectUtils = GM.Utils.GameObjectUtils;

namespace GM.Artefacts.UI
{
    public class ArtefactsUIController : GM.UI.PanelController
    {
        [Header("Prefabs")]
        public GameObject ArtefactSlotObject;

        [Header("References")]
        public Transform ArtefactsContent;

        void Start()
        {
            ArtefactData[] unlockArtefacts = App.Data.Arts.UserOwnedArtefacts;

            foreach (ArtefactData art in unlockArtefacts)
            {
                ArtefactSlot slotScript = GameObjectUtils.Instantiate<ArtefactSlot>(ArtefactSlotObject, ArtefactsContent);

                slotScript.AssignArtefact(art.Id);
            }
        }
    }
}
