using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MercID = GM.Common.Enums.MercID;

namespace GM.Mercs.Controllers
{
    public class MercComponent : MonoBehaviour
    {
        public MercID ID { get; private set; }

        protected virtual void Awake()
        {
            if (TryGetComponent(out GM.Mercs.Controllers.MercController controller))
            {
                ID = controller.ID;
            }
            else
            {
                Debug.Log("Failed to find 'MercController' on object");
            }
        }
    }
}
