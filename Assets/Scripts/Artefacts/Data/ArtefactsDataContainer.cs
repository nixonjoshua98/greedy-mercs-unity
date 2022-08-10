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
    public class ArtefactsDataContainer : Core.GMClass
    {
        private Dictionary<int, Artefact> StaticModels;
        private Dictionary<int, ArtefactUserDataModel> StateModels;

        // NB, We keep a persistant aggregated artefact class since we store state data which we do not want to save
        private Dictionary<int, AggregatedArtefactData> ArtefactsLookup = new Dictionary<int, AggregatedArtefactData>();

        public void Set(List<ArtefactUserDataModel> userArtefacts, List<Artefact> gameArtefacts)
        {
            Update(userArtefacts);
            Update(gameArtefacts);
        }

        public bool UserUnlockedAll => NumUnlockedArtefacts >= MaxArtefacts;
        public int NumUnlockedArtefacts => StateModels.Count;
        public int MaxArtefacts => StaticModels.Count;
        public double NextUnlockCost => App.Values.ArtefactUnlockCost(NumUnlockedArtefacts);
        public List<AggregatedArtefactData> Artefacts => StaticModels.Select(x => GetArtefact(x.Value.ID)).ToList();
        public List<AggregatedArtefactData> LockedArtefacts => Artefacts.Where(x => !StateModels.ContainsKey(x.Id)).ToList();

        private void Update(ArtefactUserDataModel art)
        {
            StateModels[art.ID] = art;
        }

        public void RevertBulkLevelChanges(Dictionary<int, int> artefacts)
        {
            foreach (var pair in artefacts)
            {
                GetArtefact(pair.Key).LocalLevelChange -= pair.Value;
            }
        }

        /// <summary>Update all artefacts user states</summary>
        private void Update(List<ArtefactUserDataModel> arts)
        {
            StateModels = arts.ToDictionary(x => x.ID, x => x);
        }

        /// <summary> Fetch only the static artefact game data </summary>
        public Artefact GetGameArtefact(int itemId)
        {
            return StaticModels[itemId];
        }

        public ArtefactUserDataModel GetUserArtefact(int itemId)
        {
            return StateModels[itemId];
        }


        /// <summary> Update all artefacts static data </summary>
        private void Update(List<Artefact> artefacts)
        {
            Dictionary<int, ArtefactScriptableObject> scriptables = LoadScriptableObjects();

            artefacts.ForEach(art =>
            {
                ArtefactScriptableObject scriptable = scriptables[art.ID];

                art.Icon = scriptable.Icon;
            });

            StaticModels = artefacts.ToDictionary(x => x.ID, x => x);
        }

        private Dictionary<int, ArtefactScriptableObject> LoadScriptableObjects()
        {
            return Resources.LoadAll<ArtefactScriptableObject>("Scriptables/Artefacts").ToDictionary(ele => ele.Id, ele => ele);
        }

        public List<AggregatedArtefactData> UserOwnedArtefacts => StateModels.Values.OrderBy(ele => ele.ID).Select(ele => GetArtefact(ele.ID)).ToList();
        public List<Artefact> GameArtefactsList => StaticModels.Values.ToList();
        public AggregatedArtefactData GetArtefact(int artefactId)
        {
            if (!ArtefactsLookup.TryGetValue(artefactId, out AggregatedArtefactData result))
                result = ArtefactsLookup[artefactId] = new AggregatedArtefactData(artefactId);
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

                    App.Inventory.PrestigePoints = resp.RemainingPrestigePoints;
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

                    App.Inventory.PrestigePoints -= resp.UnlockCost;

                    App.Inventory.PrestigePointsChanged.Invoke(resp.UnlockCost * -1);
                }

                call.Invoke(resp.StatusCode == HTTPCodes.Success, resp.StatusCode == HTTPCodes.Success ? GetArtefact(resp.Artefact.ID) : null);
            });
        }
    }
}
