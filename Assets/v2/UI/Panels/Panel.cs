namespace GM.UI.Panels
{
    public abstract class Panel : Core.GMMonoBehaviour
    {
        protected bool PanelIsShown;

        public virtual void OnShown() { }
        public virtual void OnHidden() { }
    }
}