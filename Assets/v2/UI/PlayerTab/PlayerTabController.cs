using UnityEngine;

namespace GM.UI
{
    public class PlayerTabController : GM.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] GameObject QuestsPopupObject;

        public void OpenQuestsPopup()
        {
            InstantiateUI<GM.Quests.UI.QuestsPopup>(QuestsPopupObject);
        }
    }
}
