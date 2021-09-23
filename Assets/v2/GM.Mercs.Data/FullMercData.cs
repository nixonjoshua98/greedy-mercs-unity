using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Mercs.Data
{
    public struct FullMercData
    {
        public MercGameData GameValues;

        public FullMercData(MercGameData values)
        {
            GameValues = values;
        }

        public MercID ID => GameValues.ID;
    }
}
