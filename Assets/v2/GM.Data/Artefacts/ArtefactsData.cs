using GM.Server;
using SimpleJSON;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;


namespace GM.Data
{
    public class ArtefactsData
    {
        UserArtefactDictionary UserData;
        Artefacts.GameArtefactData _Game => GameData.Get.Artefacts;

        public ArtefactsData(JSONNode userJSON, JSONNode gameJSON)
        {
            UserData = new UserArtefactDictionary(userJSON);
        }

        // === Properties === //
        public int NumUnlockedArtefacts => UserData.Count;
        public int MaxArtefacts => _Game.Count;
        public FullArtefactData[] Artefacts => UserData.Values.OrderBy(ele => ele.ID).Select(ele => GetArtefact(ele.ID)).ToArray();


        // === Methods === //
        public FullArtefactData GetArtefact(int artefact)
        {
            return new FullArtefactData(_Game.Get(artefact), UserData[artefact]);
        }


        // === Server Methods === //
        public void UpgradeArtefact(int artefactId, int levelsBuying, UnityAction<bool> call)
        {
            JSONNode node = new JSONObject();

            node["artefactId"] = artefactId;
            node["purchaseLevels"] = levelsBuying;

            HTTPClient.Instance.Post("artefact/upgrade", node, (code, resp) => {

                if (code == 200)
                {
                    UserData.UpdateFromJSON(resp["userArtefacts"]);

                    GM.UserData.Get.Inventory.SetServerItemData(resp["userItems"]);
                }

                call.Invoke(code == 200);
            });
        }

        public void UnlockArtefact(UnityAction<bool, int> call)
        {
            HTTPClient.Instance.Post("artefact/unlock", (code, resp) =>
            {
                if (code == 200)
                {
                    UserData.UpdateFromJSON(resp["userArtefacts"]);

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
