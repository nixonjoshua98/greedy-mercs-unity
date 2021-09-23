using System.Collections.Generic;
using System.Linq;

namespace GM.Mercs.Data
{
    public class MercUserDataDictionary : Dictionary<MercID, MercUserData>
    {
        public List<MercID> Unlocked => Keys.ToList();

        public MercUserData GetState(MercID chara)
        {
            if (!ContainsKey(chara))
            {
                base[chara] = new MercUserData() { Level = 1 };
            }

            return base[chara];
        }

        public void AddLevels(MercID chara, int levels)
        {
            MercUserData state = GetState(chara);

            state.Level += levels;
        }
    }
}
