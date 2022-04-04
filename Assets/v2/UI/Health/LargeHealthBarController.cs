using UnityEngine;

namespace GM.UI
{
    public class LargeHealthBarController : HealthBarController
    {
        [SerializeField] private GameObject InnerObjectParent;

        public void AssignHealthController(GM.Controllers.HealthController health)
        {
            Setup(health);

            InnerObjectParent.SetActive(true);
        }

        protected override void OnHealthZero()
        {
            InnerObjectParent.SetActive(false);

            Health = null;
        }
    }
}