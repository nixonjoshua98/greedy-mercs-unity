using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public static class ComponentExtensions
    {
        public static bool IsName(this Animator anim, string name) => anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
}
