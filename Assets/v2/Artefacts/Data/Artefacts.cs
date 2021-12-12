using GM.Artefacts.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using HTTPCodes = GM.HTTP.HTTPCodes;
using GM.Artefacts.Scriptables;

namespace GM.Artefacts.Data
{
    public class Artefacts : Core.GMClass
    {
        Dictionary<int, ArtefactGameDataModel> GameArtefacts;
        Dictionary<int, ArtefactUserDataModel> UserArtefacts;

        public Artefacts(List<ArtefactUserDataModel> userArtefacts, List<ArtefactGameDataModel> gameArtefacts)
        {
            Update(userArtefacts);
            Update(gameArtefacts);
        }

        public bool UserUnlockedAll => NumUnlockedArtefacts >= MaxArtefacts;
        ArtefactUserDataModel GetUserArtefact(int key) => UserArtefacts[key];
        void Update(ArtefactUserDataModel art) => UserArtefacts[art.Id] = art;
        void Update(List<ArtefactUserDataModel> arts) => UserArtefacts = arts.ToDictionary(x => x.Id, x => x);
        public int NumUnlockedArtefacts => UserArtefacts.Count;
        public int MaxArtefacts => GameArtefacts.Count;
        public ArtefactGameDataModel GetGameArtefact(int key) => GameArtefacts[key];
        void Update(List<ArtefactGameDataModel> artefacts)
        {
            GameArtefacts = artefacts.ToDictionary(x => x.Id, x => x);

            var allLocalMercData = LoadLocalData();

            foreach (var art in GameArtefacts.Values)
            {
                ArtefactScriptableObject localArtData = allLocalMercData[art.Id];

                art.Name = localArtData.Name;
                art.Icon = localArtData.Icon;
                art.IconBackground = localArtData.IconBackground;
            }
        }

        Dictionary<int, ArtefactScriptableObject> LoadLocalData() => Resources.LoadAll<ArtefactScriptableObject>("Scriptables/Artefacts").ToDictionary(ele => ele.Id, ele => ele);
        public ArtefactData[] UserOwnedArtefacts => UserArtefacts.Values.OrderBy(ele => ele.Id).Select(ele => GetArtefact(ele.Id)).ToArray();
        public List<ArtefactGameDataModel> GameArtefactsList => GameArtefacts.Values.ToList();
        public ArtefactData GetArtefact(int key) => new ArtefactData(GetGameArtefact(key), GetUserArtefact(key));


        public void UpgradeArtefact(int artefact, int levels, UnityAction<bool> call)
        {
            // Send the request to the server
            App.HTTP.UpgradeArtefact(artefact, levels, (resp) =>
            {
                // Artefact was successfully upgraded
                if (resp.StatusCode == HTTPCodes.Success)
                {
                    Update(resp.Artefact);

                    App.Data.Inv.UpdateCurrencies(resp.CurrencyItems);

                    App.Events.PrestigePointsChanged.Invoke(resp.UpgradeCost * -1);
                }

                call.Invoke(resp.StatusCode == HTTPCodes.Success);
            });
        }

        public void UnlockArtefact(UnityAction<bool, ArtefactData> call)
        {
            App.HTTP.UnlockArtefact((resp) =>
            {
                if (resp.StatusCode == HTTPCodes.Success)
                {
                    Update(resp.NewArtefact);

                    App.Data.Inv.UpdateCurrencies(resp.CurrencyItems);

                    App.Events.PrestigePointsChanged.Invoke(resp.UnlockCost * -1);
                }

                call.Invoke(resp.StatusCode == HTTPCodes.Success, resp.NewArtefact == null ? null : GetArtefact(resp.NewArtefact.Id));
            });
        }
    }
}
