namespace GM
{
    public class PrestigeIcon : PanelIcon
    {
        public override void OnClick()
        {
            CurrentStageState state = GameManager.Instance.State();

            if (state.Stage >= Common.Constants.MIN_PRESTIGE_STAGE)
                base.OnClick();
        }
    }
}