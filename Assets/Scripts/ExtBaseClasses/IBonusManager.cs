using System.Collections;
using System.Collections.Generic;

namespace GM
{
    public interface IBonusManager
    {
        List<KeyValuePair<BonusType, double>> Bonuses();
    }
}
