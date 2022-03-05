﻿using System.Collections.Generic;

namespace GM
{
    public static class IEnumerable_Extensions
    {
        public static BigDouble Sum(this IEnumerable<BigDouble> source)
        {
            BigDouble total = 0;

            foreach (BigDouble val in source)
            {
                total += val;
            }

            return total;
        }
    }
}
