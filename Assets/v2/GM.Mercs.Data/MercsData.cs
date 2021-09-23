using SimpleJSON;

namespace GM.Mercs.Data
{
    public class MercsData
    {
        MercGameDataDictionary Game;

        public MercsData(JSONNode userJSON, JSONNode gameJSON)
        {
            Game = new MercGameDataDictionary(gameJSON);
        }


        public FullMercData GetMerc(MercID merc)
        {
            return new FullMercData(Game[merc]);
        }

    }
}
