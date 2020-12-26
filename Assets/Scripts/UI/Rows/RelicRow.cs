using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class RelicRow : MonoBehaviour
{
    [SerializeField] RelicID relicId;

    public bool TryUpdate()
    {
        if (GameState.Relics.TryGetRelic(relicId, out var _))
        {
            return true;
        }

        return false;
    }
}
