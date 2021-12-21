using System.Collections.Generic;
using System.Linq;

namespace GM.Targets
{
    public class TargetList<T> : List<T> where T: Target
    {
        public bool TryGetWithType(TargetType type, ref T target)
        {
            List<T> ls = this.Where(t => t.Type == type).ToList();

            if (ls.Count > 0)
            {
                target = ls[0];
            }

            return target != null;
        }
    }
}
