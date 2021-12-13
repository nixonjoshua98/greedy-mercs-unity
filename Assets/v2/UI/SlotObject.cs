namespace GM.UI
{
    public abstract class SlotObject : Core.GMMonoBehaviour
    {
        public virtual string FormatLevel(int level) => $"Lvl. <color=orange>{level}</color>";
    }
}