using GM.Artefacts.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using HTTPCodes = GM.HTTP.HTTPCodes;
using BonusType = GM.Common.Enums.BonusType;

namespace GM.Artefacts.Data
{
    public class ArtefactsData : Core.GMClass
    {
        Dictionary<int, ArtefactGameDataModel> gameArtefacts;
        Dictionary<int, ArtefactUserDataModel> userArtefacts;

        public ArtefactsData(List<ArtefactUserDataModel> userArtefacts, List<ArtefactGameDataModel> gameArtefacts)
        {
            Update(userArtefacts);
            Update(gameArtefacts);
        }

        public bool UserUnlockedAll => NumUnlockedArtefacts >= MaxArtefacts;
        ArtefactUserDataModel GetUserArtefact(int key) => userArtefacts[key];
        void Update(ArtefactUserDataModel art) => userArtefacts[art.Id] = art;
        void Update(List<ArtefactUserDataModel> arts) => userArtefacts = arts.ToDictionary(x => x.Id, x => x);
        public int NumUnlockedArtefacts => userArtefacts.Count;
        public int MaxArtefacts => gameArtefacts.Count;
        public ArtefactGameDataModel GetGameArtefact(int key) => gameArtefacts[key];
        void Update(List<ArtefactGameDataModel> artefacts)
        {
            gameArtefacts = artefacts.ToDictionary(x => x.Id, x => x);

            var allLocalMercData = LoadLocalData();

            foreach (var art in gameArtefacts.Values)
            {
                LocalArtefactData localArtData = allLocalMercData[art.Id];

                art.Name = localArtData.Name;
                art.Icon = localArtData.Icon;
            }
        }

        Dictionary<int, LocalArtefactData> LoadLocalData() => Resources.LoadAll<LocalArtefactData>("Scriptables/Artefacts").ToDictionary(ele => ele.Id, ele => ele);
        public ArtefactData[] UserOwnedArtefacts => userArtefacts.Values.OrderBy(ele => ele.Id).Select(ele => GetArtefact(ele.Id)).ToArray();
        public ArtefactData GetArtefact(int key) => new ArtefactData(GetGameArtefact(key), GetUserArtefact(key));


        public void UpgradeArtefact(int artefact, int levels, UnityAction<bool> call)
        {
            // Server request
            var req = new HTTP.Requests.UpgradeArtefactRequest { ArtefactId = artefact, UpgradeLevels = levels };

            // Send the request to the server
            App.HTTP.Artefact_UpgradeArtefact(req, (resp) =>
            {
                // Artefact was successfully upgraded
                if (resp.StatusCode == HTTPCodes.Success)
                {
                    Update(resp.UpdatedArtefact);

                    App.Data.Inv.UpdateCurrencies(resp.CurrencyItems);

                    App.Data.Inv.E_PrestigePointsChange.Invoke(resp.UpgradeCost * -1);
                }

                call.Invoke(resp.StatusCode == HTTPCodes.Success);
            });
        }

        public void UnlockArtefact(UnityAction<bool, ArtefactData> call)
        {
            App.HTTP.Artefact_UnlockArtefact((resp) =>
            {
                if (resp.StatusCode == HTTPCodes.Success)
                {
                    Update(resp.NewArtefact);

                    App.Data.Inv.UpdateCurrencies(resp.CurrencyItems);

                    App.Data.Inv.E_PrestigePointsChange.Invoke(resp.UnlockCost * -1);
                }

                call.Invoke(resp.StatusCode == HTTPCodes.Success, resp.NewArtefact == null ? null : GetArtefact(resp.NewArtefact.Id));
            });
        }
    }
}
