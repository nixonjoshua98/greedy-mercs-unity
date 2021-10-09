using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine;
using GM.Artefacts.Models;
using GM.Extensions;


namespace GM.Artefacts.Data
{
    public class ArtefactsData : Core.GMClass
    {
        List<ArtefactGameDataModel> GameArtefactsList;
        List<ArtefactUserDataModel> UserArtefactsList;

        public ArtefactsData(List<ArtefactUserDataModel> userArtefacts, List<ArtefactGameDataModel> gameArtefacts)
        {
            UserArtefactsList = userArtefacts;

            UpdateAllGameArtefacts(gameArtefacts);
        }


        // == User == //
        public ArtefactUserDataModel GetUserArtefact(int key) => UserArtefactsList.Where(art => art.Id == key).FirstOrDefault();
        public void UpdateUserArtefact(ArtefactUserDataModel art) => UserArtefactsList.UpdateOrInsertElement(art, (ele) => ele.Id == art.Id);
        public int NumUnlockedArtefacts => UserArtefactsList.Count;


        // == Game == //
        public int MaxArtefacts => GameArtefactsList.Count;
        public ArtefactGameDataModel GetGameArtefact(int key) => GameArtefactsList.Where(art => art.Id == key).FirstOrDefault();
        void UpdateAllGameArtefacts(List<ArtefactGameDataModel> artefacts)
        {
            GameArtefactsList = artefacts;

            var allLocalMercData = LoadLocalData();

            foreach (var art in GameArtefactsList)
            {
                LocalArtefactData localArtData = allLocalMercData[art.Id];

                art.Name = localArtData.Name;
                art.Icon = localArtData.Icon;
                art.Slot = localArtData.Slot;
            }
        }


        // == Local == //
        Dictionary<int, LocalArtefactData> LoadLocalData() => Resources.LoadAll<LocalArtefactData>("Artefacts").ToDictionary(ele => ele.Id, ele => ele);


        // == Combined == //
        public ArtefactData[] UserOwnedArtefacts => UserArtefactsList.OrderBy(ele => ele.Id).Select(ele => GetArtefact(ele.Id)).ToArray();
        public ArtefactData GetArtefact(int key) => new ArtefactData(GetGameArtefact(key), GetUserArtefact(key));


        public void UpgradeArtefact(int artefact, int levels, UnityAction<bool> call)
        {
            var req = new HTTP.Requests.UpgradeArtefactRequest { ArtefactId = artefact, UpgradeLevels = levels };

            App.HTTP.Artefact_UpgradeArtefact(req, (resp) =>
            {
                if (resp.StatusCode == 200)
                {
                    UpdateUserArtefact(resp.UpdatedArtefact);

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
                    UpdateUserArtefact(resp.NewArtefact);

                    App.Data.Inv.UpdateCurrencies(resp.CurrencyItems);
                }

                call.Invoke(resp.StatusCode == 200, resp.StatusCode == 200 ? resp.NewArtefact.Id : -1);
            });
        }


        // === Special Methods === //
        public List<KeyValuePair<BonusType, double>> Bonuses()
        {
            List<KeyValuePair<BonusType, double>> ls = new List<KeyValuePair<BonusType, double>>();

            foreach (ArtefactData state in UserOwnedArtefacts)
            {
                ls.Add(new KeyValuePair<BonusType, double>(state.Bonus, state.BaseEffect));
            }

            return ls;
        }
    }
}
