using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using BountyStaticData = BountyData.BountyStaticData;

namespace BountyUI
{
    public class BountyIcon : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Text bountyName;
        [SerializeField] Text info;

        public void SetBounty(BountyStaticData bounty)
        {
            bountyName.text = bounty.name;

            info.text = string.Format("{0} Points / hour", bounty.bountyPoints);
        }
    }
}