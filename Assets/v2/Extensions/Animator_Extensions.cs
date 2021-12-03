using UnityEngine;

namespace GM
{
    public static class Animator_Extensions
    {
        public static bool IsName(this Animator source, string name) => source.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
}
