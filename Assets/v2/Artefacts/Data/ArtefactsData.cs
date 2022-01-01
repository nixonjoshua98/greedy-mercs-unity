using GM.Artefacts.Models;
using GM.Artefacts.Scriptables;
using GM.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using HTTPCodes = GM.HTTP.HTTPCodes;

namespace GM.Artefacts.Data
{
    public class ArtefactsData : Core.GMClass
    {
        Dictionary<int, ArtefactGameDataModel> StaticArtefactData;
        Dictionary<int, ArtefactUserDataModel> ArtefactStates;

        public ArtefactsData(List<ArtefactUserDataModel> userArtefacts, List<ArtefactGameDataModel> gameArtefacts)
        {
            Update(userArtefacts);
            Update(gameArtefacts);
        }

        public bool UserUnlockedAll => NumUnlockedArtefacts >= MaxArtefacts;
        public int NumUnlockedArtefacts => ArtefactStates.Count;
        public int MaxArtefacts => StaticArtefactData.Count;
        void Update(ArtefactUserDataModel art) => ArtefactStates[art.Id] = art;

        public void UpdateAllData(List<ArtefactUserDataModel> userArtefacts, List<ArtefactGameDataModel> staticArtefacts)
        {
            Update(userArtefacts);
            Update(staticArtefacts);
        }

        /// <summary>Update all artefacts user states</summary>
        void Update(List<ArtefactUserDataModel> arts) => ArtefactStates = arts.ToDictionary(x => x.Id, x => x);

        /// <summary> Fetch only the static artefact game data </summary>
        public ArtefactGameDataModel GetGameArtefact(int itemId) => StaticArtefactData[itemId];

        /// <summary> Update all artefacts static data </summary>
        void Update(List<ArtefactGameDataModel> artefacts)
        {
            Dictionary<int, ArtefactScriptableObject> scriptables = LoadScriptableObjects();

            artefacts.ForEach(art =>
            {
                ArtefactScriptableObject scriptable = scriptables[art.Id];

                art.Name = scriptable.Name;
                art.Icon = scriptable.Icon;
                art.IconBackground = scriptable.IconBackground;
            });

            StaticArtefactData = artefacts.ToDictionary(x => x.Id, x => x);
        }

        Dictionary<int, ArtefactScriptableObject> LoadScriptableObjects() => Resources.LoadAll<ArtefactScriptableObject>("Scriptables/Artefacts").ToDictionary(ele => ele.Id, ele => ele);
        public List<ArtefactData> UserOwnedArtefacts => ArtefactStates.Values.OrderBy(ele => ele.Id).Select(ele => GetArtefact(ele.Id)).ToList();
        public List<ArtefactGameDataModel> GameArtefactsList => StaticArtefactData.Values.ToList();
        public ArtefactData GetArtefact(int itemId) => new ArtefactData(StaticArtefactData[itemId], ArtefactStates[itemId]);

        // = Data = //
        public void Serialize<T> (ref T model) where T : IServerUserData
        {
            model.Artefacts = ArtefactStates.Values.ToList();
        }

        // = Server Requests = //
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
                    Update(resp.Artefact);

                    App.Data.Inv.UpdateCurrencies(resp.CurrencyItems);

                    App.Events.PrestigePointsChanged.Invoke(resp.UnlockCost * -1);
                }

                call.Invoke(resp.StatusCode == HTTPCodes.Success, resp.StatusCode == HTTPCodes.Success ? GetArtefact(resp.Artefact.Id) : null);
            });
        }
    }
}
