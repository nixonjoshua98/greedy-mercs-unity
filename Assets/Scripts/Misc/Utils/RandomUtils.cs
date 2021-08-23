using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace GM
{
    public static class RandomUtils 
    {
        public static List<T> Shuffle<T>(List<T> ls, int seed)
        {
            Random r = new Random(seed);

            return ls.OrderBy(x => (r.Next())).ToList();
        }
    }
}
