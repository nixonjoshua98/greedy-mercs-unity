using SimpleJSON;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;


namespace GM.Artefacts.Data
{
    public class ArtefactsData : Core.GMClass
    {
        UserArtefactsCollection User;
        GameArtefactsCollection _Game;

        public ArtefactsData(JSONNode userJSON, JSONNode gameJSON)
        {
            User = new UserArtefactsCollection(userJSON);
            _Game = new GameArtefactsCollection(gameJSON);
        }

        public int NumUnlockedArtefacts => User.Count;
        public int MaxArtefacts => _Game.Count;
        public FullArtefactData[] Artefacts => User.Values.OrderBy(ele => ele.Id).Select(ele => GetArtefact(ele.Id)).ToArray();


        // === Methods === //
        public FullArtefactData GetArtefact(int artefact)
        {
            return new FullArtefactData(_Game[artefact], User[artefact]);
        }


        public void UpgradeArtefact(int artefact, int levels, UnityAction<bool> call)
        {
            var req = new HTTP.Requests.UpgradeArtefactRequest { ArtefactId = artefact, UpgradeLevels = levels };

            App.HTTP.UpgradeArtefact(req, (resp) =>
            {
                if (resp.StatusCode == 200)
                {
                    User.Update(resp.UserArtefacts);

                    App.Data.Inv.UpdateCurrencies(resp.UserCurrencies);
                }

                call.Invoke(resp.StatusCode == 200);
            });
        }

        public void UnlockArtefact(UnityAction<bool, int> call)
        {
            App.HTTP.UnlockArtefact((resp) =>
            {
                if (resp.StatusCode == 200)
                {
                    User.Update(resp.UserArtefacts);

                    App.Data.Inv.UpdateCurrencies(resp.UserCurrencies);
                }

                call.Invoke(resp.StatusCode == 200, resp.NewArtefactId);
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
