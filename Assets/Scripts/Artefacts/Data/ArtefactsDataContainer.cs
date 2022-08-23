using SRC.Artefacts.Scriptables;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using HTTPCodes = SRC.HTTP.HTTPCodes;

namespace SRC.Artefacts.Data
{
    public class ArtefactsDataContainer : Core.GMClass
    {
        private List<Artefact> DataFiles;
        private List<UserArtefact> StateModels;

        public readonly List<AggregatedArtefactData> Artefacts = new();

        public void Set(List<UserArtefact> userArtefacts, List<Artefact> gameArtefacts)
        {
            StateModels = userArtefacts;
            Update(gameArtefacts);
        }

        public bool UserUnlockedAll => NumUnlockedArtefacts >= MaxArtefacts;
        public int NumUnlockedArtefacts => StateModels.Count;
        public int MaxArtefacts => DataFiles.Count;
        public double NextUnlockCost => App.Bonuses.ArtefactUnlockCost(NumUnlockedArtefacts);

        public List<AggregatedArtefactData> LockedArtefacts =>
            DataFiles
            .Where(x => !StateModels.Exists(art => art.ID == x.ArtefactID))
            .Select(art => GetArtefact(art.ArtefactID))
            .ToList();

        public List<AggregatedArtefactData> UnlockedArtefacts =>
            DataFiles
            .Where(x => StateModels.Exists(art => art.ID == x.ArtefactID))
            .Select(art => GetArtefact(art.ArtefactID))
            .ToList();

        public void RevertBulkLevelChanges(Dictionary<int, int> artefacts)
        {
            foreach (var pair in artefacts)
            {
                GetArtefact(pair.Key).LocalLevelChange -= pair.Value;
            }
        }

        private void Update(List<Artefact> artefacts)
        {
            Dictionary<int, ArtefactScriptableObject> scriptables = LoadScriptableObjects();

            artefacts.ForEach(art =>
            {
                ArtefactScriptableObject scriptable = scriptables[art.ArtefactID];

                art.Icon = scriptable.Icon;
            });

            DataFiles = artefacts;
        }

        private Dictionary<int, ArtefactScriptableObject> LoadScriptableObjects()
        {
            return Resources.LoadAll<ArtefactScriptableObject>("Scriptables/Artefacts").ToDictionary(ele => ele.Id, ele => ele);
        }

        public AggregatedArtefactData GetArtefact(int artefactId)
        {
            return Artefacts.GetOrCreate(art => art.ArtefactID == artefactId, () =>
            {
                Func<Artefact> getArtefact = () => DataFiles.FirstOrDefault(art => art.ArtefactID == artefactId);
                Func<UserArtefact> getUserArtefacts = () => StateModels.FirstOrDefault(art => art.ID == artefactId);

                return new(artefactId, getArtefact, getUserArtefacts);
            });
        }

        public void BulkUpgradeArtefact(Dictionary<int, int> artefacts, Action onRequestReceived, Action<bool> call)
        {
            App.HTTP.BulkUpgradeArtefacts(artefacts, (resp) =>
            {
                onRequestReceived?.Invoke();

                if (resp.StatusCode == HTTPCodes.Success)
                {
                    StateModels = resp.Artefacts;

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
                    StateModels[resp.Artefact.ID] = resp.Artefact;

                    App.Inventory.PrestigePoints -= resp.UnlockCost;

                    App.Inventory.PrestigePointsChanged.Invoke(resp.UnlockCost * -1);
                }

                call.Invoke(resp.StatusCode == HTTPCodes.Success, resp.StatusCode == HTTPCodes.Success ? GetArtefact(resp.Artefact.ID) : null);
            });
        }
    }
}
