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
        Dictionary<int, ArtefactGameDataModel> StaticArtefactData;
        Dictionary<int, ArtefactUserDataModel> ArtefactStates;

        public Artefacts(List<ArtefactUserDataModel> userArtefacts, List<ArtefactGameDataModel> gameArtefacts)
        {
            Update(userArtefacts);
            Update(gameArtefacts);
        }

        public bool UserUnlockedAll => NumUnlockedArtefacts >= MaxArtefacts;
        public int NumUnlockedArtefacts => ArtefactStates.Count;
        public int MaxArtefacts => StaticArtefactData.Count;
        ArtefactUserDataModel GetUserArtefact(int key) => ArtefactStates[key];
        void Update(ArtefactUserDataModel art) => ArtefactStates[art.Id] = art;

        /// <summary>Update all artefacts user states</summary>
        void Update(List<ArtefactUserDataModel> arts) => ArtefactStates = arts.ToDictionary(x => x.Id, x => x);

        /// <summary>Fetch only the static artefact game data</summary>
        public ArtefactGameDataModel GetGameArtefact(int key) => StaticArtefactData[key];

        /// <summary>Update all artefacts static data</summary>
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
                    Update(resp.Artefact);

                    App.Data.Inv.UpdateCurrencies(resp.CurrencyItems);

                    App.Events.PrestigePointsChanged.Invoke(resp.UnlockCost * -1);
                }

                call.Invoke(resp.StatusCode == HTTPCodes.Success, resp.StatusCode == HTTPCodes.Success ? null : GetArtefact(resp.Artefact.Id));
            });
        }
    }
}
