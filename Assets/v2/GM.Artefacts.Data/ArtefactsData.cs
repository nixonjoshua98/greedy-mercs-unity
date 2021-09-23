using SimpleJSON;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;


namespace GM.Artefacts.Data
{
    public class ArtefactsData : Core.GMClass
    {
        UserArtefactsDictionary _User;
        GameArtefactsDictionary _Game;

        public ArtefactsData(JSONNode userJSON, JSONNode gameJSON)
        {
            _User = new UserArtefactsDictionary(userJSON);
            _Game = new GameArtefactsDictionary(gameJSON);
        }

        // === Properties === //
        public int NumUnlockedArtefacts => _User.Count;
        public int MaxArtefacts => _Game.Count;
        public FullArtefactData[] Artefacts => _User.Values.OrderBy(ele => ele.ID).Select(ele => GetArtefact(ele.ID)).ToArray();


        // === Methods === //
        public FullArtefactData GetArtefact(int artefact)
        {
            return new FullArtefactData(_Game[artefact], _User[artefact]);
        }


        // === Update === //
        public void Update(JSONNode userJSON, JSONNode gameJSON)
        {
            _User.UpdateFromJSON(userJSON);
            _Game.UpdateFromJSON(gameJSON);
        }


        // === Server Methods === //
        public void UpgradeArtefact(int artefactId, int levelsBuying, UnityAction<bool> call)
        {
            JSONNode node = new JSONObject();

            node["artefactId"] = artefactId;
            node["purchaseLevels"] = levelsBuying;

            App.HTTP.Post("artefact/upgrade", node, (code, resp) => {

                if (code == 200)
                {
                    _User.UpdateFromJSON(resp["userArtefacts"]);

                    GM.UserData.Get.Inventory.SetServerItemData(resp["userItems"]);
                }

                call.Invoke(code == 200);
            });
        }

        public void UnlockArtefact(UnityAction<bool, int> call)
        {
            App.HTTP.Post("artefact/unlock", (code, resp) =>
            {
                if (code == 200)
                {
                    _User.UpdateFromJSON(resp["userArtefacts"]);

                    GM.UserData.Get.Inventory.SetServerItemData(resp["userItems"]);
                }

                call.Invoke(code == 200, code == 200 ? resp["newArtefactId"].AsInt : -1);
            });
        }


        // === Special Methods === //
        public List<KeyValuePair<BonusType, double>> Bonuses()
        {
            List<KeyValuePair<BonusType, double>> ls = new List<KeyValuePair<BonusType, double>>();

            foreach (FullArtefactData state in Artefacts)
            {
                ls.Add(new KeyValuePair<BonusType, double>(state.Values.Bonus, state.BaseEffect));
            }

            return ls;
        }
    }
}
