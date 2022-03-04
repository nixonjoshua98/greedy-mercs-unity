using GM.Armoury.Data;
using GM.Artefacts.Data;
using GM.Bounties.Data;
using GM.BountyShop.Data;
using GM.Common.Interfaces;
using GM.CurrencyItems.Data;
using GM.Inventory.Data;
using GM.Mercs.Data;
using GM.Upgrades.Data;
using System;
using GM.LocalFiles;

namespace GM.Core
{
    public class GMData : GMClass
    {
        public MercsData Mercs;
        public ArmouryData Armoury;
        public UserInventory Inv;
        public ArtefactsData Artefacts;
        public ItemsData Items;
        public BountiesData Bounties;
        public BountyShopData BountyShop;
        public PlayerUpgrades Upgrades;

        public CurrentPrestigeState GameState;

        public DateTime NextDailyReset;

        public GMData(IServerUserData userData, IStaticGameData staticData, LocalSaveFileModel localSaveFile, PersistantLocalFile persistLocalFile)
        {
            NextDailyReset = staticData.NextDailyReset;

            // = Scriptable Object Models = //
            Items = new ItemsData();

            // = Local Models = //
            GameState = CurrentPrestigeState.Deserialize(localSaveFile);

            Upgrades    = new PlayerUpgrades();
            Inv         = new UserInventory(userData.CurrencyItems);
            Mercs       = new MercsData(userData, staticData, localSaveFile, persistLocalFile);
            Artefacts   = new ArtefactsData(userData.Artefacts, staticData.Artefacts);
            Armoury     = new ArmouryData(userData.ArmouryItems, staticData.Armoury);
            Bounties    = new BountiesData(userData.BountyData, staticData.Bounties);
            BountyShop  = new BountyShopData(userData.BountyShop);
        }

        public LocalSaveFileModel CreateLocalSaveFile()
        {
            LocalSaveFileModel savefile = new LocalSaveFileModel();

            GameState.UpdateLocalSaveFile(ref savefile);
            Mercs.UpdateLocalSaveFile(ref savefile);

            return savefile;
        }

        public void DeleteSoftUserData()
        {
            GameState = new CurrentPrestigeState();

            Mercs.DeleteSoftData();

            Upgrades.ResetLevels();
            Inv.ResetLocalResources();
        }

        public void Update(IServerUserData userData, IStaticGameData staticData)
        {
            LocalSaveFileModel savefile = App.SaveManager.LoadSaveFile();

            Mercs.Update(userData, staticData, savefile);

            BountyShop  = new BountyShopData(userData.BountyShop);

            Inv.UpdateCurrencies(userData.CurrencyItems);
            Artefacts.UpdateAllData(userData.Artefacts, staticData.Artefacts);
            Armoury.UpdateAllData(userData.ArmouryItems, staticData.Armoury);
            Bounties.UpdateAllData(userData.BountyData, staticData.Bounties);
        }
    }
}