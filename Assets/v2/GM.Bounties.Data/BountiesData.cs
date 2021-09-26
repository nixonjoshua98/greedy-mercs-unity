using SimpleJSON;

namespace GM.Bounties.Data
{
    public class BountiesData
    {
        public GameBountiesDataDictionary Game;

        public BountiesData(JSONNode gameJSON)
        {
            Game = new GameBountiesDataDictionary(gameJSON);
        }
    }
}
