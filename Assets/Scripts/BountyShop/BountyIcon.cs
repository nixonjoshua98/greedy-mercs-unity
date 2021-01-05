using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace BountyUI
{
    public class BountyIcon : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Text bountyName;
        [SerializeField] Text info;
        [SerializeField] Image icon;
        [SerializeField] Button button;

        int bountyIndex;

        public void SetBountyIndex(int _bountyIndex)
        {
            bountyIndex = _bountyIndex;

            var staticData = StaticData.Bounties.Get(bountyIndex);

            bountyName.text = staticData.name;

            info.text = string.Format("{0} Points / hour", staticData.bountyPoints);
        }
    }
}