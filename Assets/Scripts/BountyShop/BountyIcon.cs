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
        [SerializeField] Image icon;
        [SerializeField] Text info;

        public void SetBounty(ScriptableBounty bounty)
        {
            bountyName.text = bounty.name;

            info.text = string.Format("{0} Points / hour", bounty.bountyPoints);

            icon.sprite = bounty.icon;
        }
    }
}