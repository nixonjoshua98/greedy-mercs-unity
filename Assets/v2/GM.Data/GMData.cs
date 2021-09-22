using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace GM.Data
{
    public class GMData
    {
        public ArtefactsData Arts;

        public GMData(JSONNode userJSON, JSONNode gameJSON)
        {
            Arts = new ArtefactsData(userJSON["artefacts"], gameJSON["artefactsResources"]);
        }
    }
}
