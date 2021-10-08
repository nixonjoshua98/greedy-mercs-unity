using SimpleJSON;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;


namespace GM.Artefacts.Data
{
    public class ArtefactsData : Core.GMClass
    {
        UserArtefactsCollection User;
        GameArtefactsCollection Game;

        public ArtefactsData(JSONNode userJSON, JSONNode gameJSON)
        {
            User = new UserArtefactsCollection(userJSON);
            Game = new GameArtefactsCollection(gameJSON);
        }

        public ArtefactsData(List<Models.UserArtefactModel> userArtefacts, List<Models.ArtefactGameDataModel> gameArtefacts)
        {
            User = new UserArtefactsCollection(userArtefacts);
            Game = new GameArtefactsCollection(gameArtefacts);
        }


        public int NumUnlockedArtefacts => User.Count;
        public int MaxArtefacts => Game.Count;
        public FullArtefactData[] Artefacts => User.List.OrderBy(ele => ele.Id).Select(ele => GetArtefact(ele.Id)).ToArray();


        // === Methods === //
        public FullArtefactData GetArtefact(int key)
        {
            return new FullArtefactData(Game.Get(key), User.Get(key));
        }


        public void UpgradeArtefact(int artefact, int levels, UnityAction<bool> call)
        {
            var req = new HTTP.Requests.UpgradeArtefactRequest { ArtefactId = artefact, UpgradeLevels = levels };

            App.HTTP.Artefact_UpgradeArtefact(req, (resp) =>
            {
                if (resp.StatusCode == 200)
                {
                    User.Update(resp.UpdatedArtefact);

                    App.Data.Inv.UpdateCurrencies(resp.CurrencyItems);
                }

                call.Invoke(resp.StatusCode == 200);
            });
        }

        public void UnlockArtefact(UnityAction<bool, int> call)
        {
            App.HTTP.Artefact_UnlockArtefact((resp) =>
            {
                if (resp.StatusCode == 200)
                {
                    User.Update(resp.NewArtefact);

                    App.Data.Inv.UpdateCurrencies(resp.CurrencyItems);
                }

                call.Invoke(resp.StatusCode == 200, resp.StatusCode == 200 ? resp.NewArtefact.Id : -1);
            });
        }


        // === Special Methods === //
        public List<KeyValuePair<BonusType, double>> Bonuses()
        {
            List<KeyValuePair<BonusType, double>> ls = new List<KeyValuePair<BonusType, double>>();

            foreach (FullArtefactData state in Artefacts)
            {
                ls.Add(new KeyValuePair<BonusType, double>(state.Game.Bonus, state.BaseEffect));
            }

            return ls;
        }
    }
}
