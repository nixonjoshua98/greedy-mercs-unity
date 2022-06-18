using System;
using System.Collections.Generic;

namespace GM.Artefacts
{
    internal class BulkUpgradeChanges
    {
        public double UpgradeCost = 0;
        public Dictionary<int, int> Upgrades = new Dictionary<int, int>();
    }

    public class BulkUpgradeController : GM.Core.GMClass
    {
        private DateTime FirstUpdateTime;
        private readonly Action<bool> UpgradeCallback;
        private readonly float RequestInterval;
        private bool WaitingForResponse;
        private BulkUpgradeChanges UnprocessedChanges;
        private BulkUpgradeChanges RequestChanges;

        public BulkUpgradeController(Action<bool> success, float interval = 3.0f)
        {
            UpgradeCallback = success;
            RequestInterval = interval;
        }

        public bool RequestIsReady => (DateTime.UtcNow - FirstUpdateTime).TotalSeconds > RequestInterval && !WaitingForResponse && UnprocessedChanges != null;

        public void Add(int artefact, int levels)
        {
            if (UnprocessedChanges == null)
            {
                FirstUpdateTime = DateTime.UtcNow;

                UnprocessedChanges = new BulkUpgradeChanges();
            }

            UnprocessedChanges.Upgrades[artefact] = UnprocessedChanges.Upgrades.Get(artefact, 0) + levels;

            var upgradeCost = App.Values.ArtefactUpgradeCost(artefact, levels);

            // Currency changes
            UnprocessedChanges.UpgradeCost += upgradeCost;
            App.Inventory.PrestigePoints -= upgradeCost;

            // Artefact upgrade change
            App.Artefacts.GetArtefact(artefact).LocalLevelChange += levels;
        }

        public void Process()
        {
            if (RequestIsReady)
            {
                WaitingForResponse = true;
                RequestChanges = UnprocessedChanges;
                UnprocessedChanges = null;

                App.Artefacts.BulkUpgradeArtefact(RequestChanges.Upgrades, OnResponseReceived, UpgradeCallback);
            }
        }

        private void OnResponseReceived()
        {
            WaitingForResponse = false;

            App.Artefacts.RevertBulkLevelChanges(RequestChanges.Upgrades);

            App.Inventory.PrestigePoints += RequestChanges.UpgradeCost;
        }
    }
}
