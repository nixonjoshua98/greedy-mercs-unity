using System;
using System.Collections.Generic;
using System.Numerics;

namespace GM.Artefacts
{
    class BulkUpgradeChanges
    {
        public BigInteger UpgradeCost = 0;
        public Dictionary<int, int> Upgrades = new Dictionary<int, int>();
    }

    public class BulkUpgradeController: GM.Core.GMClass
    {
        DateTime FirstUpdateTime;
        Action<bool> UpgradeCallback;
        float RequestInterval;
        bool WaitingForResponse;

        BulkUpgradeChanges UnprocessedChanges;
        BulkUpgradeChanges RequestChanges;

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

            var upgradeCost = App.GMCache.ArtefactUpgradeCost(artefact, levels);

            // Currency changes
            UnprocessedChanges.UpgradeCost += upgradeCost;
            App.GMData.Inv.PrestigePoints -= upgradeCost;

            // Artefact upgrade change
            App.GMData.Artefacts.GetArtefact(artefact).LocalLevelChange += levels;
        }

        public void Process()
        {
            if (RequestIsReady)
            {
                WaitingForResponse  = true;
                RequestChanges      = UnprocessedChanges;
                UnprocessedChanges  = null;

                App.GMData.Artefacts.BulkUpgradeArtefact(RequestChanges.Upgrades, OnResponseReceived, UpgradeCallback);
            }
        }

        void OnResponseReceived()
        {
            WaitingForResponse = false;

            App.GMData.Artefacts.RevertBulkLevelChanges(RequestChanges.Upgrades);

            App.GMData.Inv.PrestigePoints += RequestChanges.UpgradeCost;
        }
    }
}
