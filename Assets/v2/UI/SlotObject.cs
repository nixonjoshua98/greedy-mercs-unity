namespace GM.UI
{
    public abstract class SlotObject : Core.GMMonoBehaviour
    {
        public virtual string FormatLevel(int level)
        {
            return $"Lvl. <color=orange>{level}</color>";
        }
    }
}