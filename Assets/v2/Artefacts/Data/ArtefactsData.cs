using GM.Artefacts.Models;
using GM.Artefacts.Scriptables;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using HTTPCodes = GM.HTTP.HTTPCodes;

namespace GM.Artefacts.Data
{
    public class ArtefactsData : Core.GMClass
    {
        Dictionary<int, ArtefactGameDataModel> StaticModels;
        Dictionary<int, ArtefactUserDataModel> StateModels;

        Dictionary<int, AggregatedArtefactData> Artefacts = new Dictionary<int, AggregatedArtefactData>();

        public void Set(List<ArtefactUserDataModel> userArtefacts, List<ArtefactGameDataModel> gameArtefacts)
        {
            Update(userArtefacts);
            Update(gameArtefacts);
        }

        public bool UserUnlockedAll => NumUnlockedArtefacts >= MaxArtefacts;
        public int NumUnlockedArtefacts => StateModels.Count;
        public int MaxArtefacts => StaticModels.Count;
        void Update(ArtefactUserDataModel art) => StateModels[art.ID] = art;

        public void UpdateAllData(List<ArtefactUserDataModel> userArtefacts, List<ArtefactGameDataModel> staticArtefacts)
        {
            Update(userArtefacts);
            Update(staticArtefacts);
        }

        public void RevertBulkLevelChanges(Dictionary<int, int> artefacts)
        {
            foreach (var pair in artefacts)
            {
                GetArtefact(pair.Key).LocalLevelChange -= pair.Value;
            }
        }

        /// <summary>Update all artefacts user states</summary>
        void Update(List<ArtefactUserDataModel> arts) => StateModels = arts.ToDictionary(x => x.ID, x => x);

        /// <summary> Fetch only the static artefact game data </summary>
        public ArtefactGameDataModel GetGameArtefact(int itemId) => StaticModels[itemId];

        public ArtefactUserDataModel GetUserArtefact(int itemId) => StateModels[itemId];


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

            StaticModels = artefacts.ToDictionary(x => x.Id, x => x);
        }

        Dictionary<int, ArtefactScriptableObject> LoadScriptableObjects() => Resources.LoadAll<ArtefactScriptableObject>("Scriptables/Artefacts").ToDictionary(ele => ele.Id, ele => ele);
        public List<AggregatedArtefactData> UserOwnedArtefacts => StateModels.Values.OrderBy(ele => ele.ID).Select(ele => GetArtefact(ele.ID)).ToList();
        public List<ArtefactGameDataModel> GameArtefactsList => StaticModels.Values.ToList();
        public AggregatedArtefactData GetArtefact(int artefactId)
        {
            if (!Artefacts.TryGetValue(artefactId, out AggregatedArtefactData result))
                result = Artefacts[artefactId] = new AggregatedArtefactData(artefactId);
            return result;
        }


        public void BulkUpgradeArtefact(Dictionary<int, int> artefacts, Action onRequestReceived, Action<bool> call)
        {
            App.HTTP.BulkUpgradeArtefacts(artefacts, (resp) =>
            {
                onRequestReceived?.Invoke();

                if (resp.StatusCode == HTTPCodes.Success)
                {
                    Update(resp.Artefacts);

                    App.Inventory.PrestigePoints = resp.PrestigePoints;
                }

                call.Invoke(resp.StatusCode == HTTPCodes.Success);
            });
        }

        public void UnlockArtefact(UnityAction<bool, AggregatedArtefactData> call)
        {
            App.HTTP.UnlockArtefact((resp) =>
            {
                if (resp.StatusCode == HTTPCodes.Success)
                {
                    Update(resp.Artefact);

                    App.Inventory.UpdateCurrencies(resp.CurrencyItems);

                    App.Events.PrestigePointsChanged.Invoke(resp.UnlockCost * -1);
                }

                call.Invoke(resp.StatusCode == HTTPCodes.Success, resp.StatusCode == HTTPCodes.Success ? GetArtefact(resp.Artefact.ID) : null);
            });
        }
    }
}
