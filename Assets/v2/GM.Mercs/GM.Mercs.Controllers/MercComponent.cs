using UnityEngine;
using MercID = GM.Common.Enums.MercID;

namespace GM.Mercs.Controllers
{
    public class MercComponent : MonoBehaviour
    {
        public MercID ID { get; private set; }

        protected Animator AvatarAnimator;
        protected MercMovement Movement;
        protected AnimationStrings Animations;

        protected virtual void Awake()
        {
            if (TryGetComponent(out MercController controller))
            {
                ID = controller.ID;
                Movement = controller.Movement;
                AvatarAnimator = controller.AvatarAnimator;
                Animations = controller.Animations;
            }
            else
            {
                Debug.Log("Failed to find 'MercController' on object");
            }
        }
    }
}
