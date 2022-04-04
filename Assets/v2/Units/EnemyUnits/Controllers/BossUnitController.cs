using UnityEngine;

namespace GM.Units.Controllers
{
    public class BossUnitController : EnemyUnitController
    {
        protected override void LinkHealthBar()
        {
            GameObject obj = GameObject.FindGameObjectWithTag(GM.Common.Constants.Tags.BossUnitHealthBar);

            if (obj != null && obj.TryGetComponent(out GM.UI.LargeHealthBarController healthbar))
            {
                healthbar.AssignHealthController(HealthController);
            }
            else
            {
                GMLogger.WhenNull(obj, $"'{GM.Common.Constants.Tags.BossUnitHealthBar}' object with tag not found in scene");

                base.LinkHealthBar();
            }
        }
    }
}
