using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SRC
{
    public static class ColorExtensions
    {
        public static string ToHex(this Color col)
        {
            return $"#{(int)(col.r * 255):X2}{(int)(col.g * 255):X2}{(int)(col.b * 255):X2}";
        }
    }
}
