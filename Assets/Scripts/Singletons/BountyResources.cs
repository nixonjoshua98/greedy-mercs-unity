using System.Linq;
using System.Collections.Generic;

using UnityEngine;


public class BountyResources : MonoBehaviour
{
    static BountyResources Instance = null;

    public List<BountySO> bountyList;

    public static List<int> Keys { get { return Instance.bountyList.Select(o => o.BountyID).ToList(); } }


    void Awake()
    {
        Instance = this;

        foreach (var row in bountyList)
            row.OnAwake();
    }

    void Start()
    {
    }

    public static BountySO Get(int bountyIndex)
    {
        return Instance.bountyList[bountyIndex];
    }

    public static bool GetStageBoss(int stage, out BountySO bounty)
    {
        bounty = null;

        foreach (var b in Instance.bountyList)
        {
            if (b.unlockStage == stage)
            {
                bounty = b;

                return true;
            }
        }

        return false;
    }
}