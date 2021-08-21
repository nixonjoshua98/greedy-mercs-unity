using SimpleJSON;
using System.Numerics;

namespace GM.Data
{
    static class InventoryKeys
    {
        public static string Local_Gold     = "local_Gold";
        public static string Local_Energy   = "local_Energy";

        public static string Server_Gems            = "blueGems";
        public static string Server_PrestigePoints  = "prestigePoints";
        public static string Server_BountyPoints    = "bountyPoints";
        public static string Server_ArmouryPoints   = "ironIngots";
    }


    public class UserInventory : ILocalDataContainer
    {
        #region Constants
        const string LOCAL_FILE_NAME = "local/_c673_inv_56f_user";
        #endregion

        public BigInteger PrestigePoints;

        public long BountyPoints;
        public long IronIngots;
        public long BlueGems;

        public float Energy;
        public BigDouble Gold;

        public UserInventory(JSONNode node)
        {
            LoadLocalSaveFile();

            SetServerItemData(node["items"]);
        }


        public void SetServerItemData(JSONNode node)
        {
            BlueGems        = node.GetValueOrDefault(InventoryKeys.Server_Gems, 0).AsInt;
            IronIngots      = node.GetValueOrDefault(InventoryKeys.Server_ArmouryPoints, 0).AsInt;
            BountyPoints    = node.GetValueOrDefault(InventoryKeys.Server_BountyPoints, 0).AsInt;

            PrestigePoints = BigInteger.Parse(node.GetValueOrDefault(InventoryKeys.Server_PrestigePoints, 0), System.Globalization.NumberStyles.Any);
        }


        // = = = Local Save = = = //

        void LoadLocalSaveFile()
        {
            FileUtils.LoadJSON(FileUtils.ResolvePath(LOCAL_FILE_NAME), out JSONNode result);

            Energy  = result.GetValueOrDefault(InventoryKeys.Local_Energy, 0).AsFloat;
            Gold    = BigDouble.Parse(result.GetValueOrDefault(InventoryKeys.Local_Gold, 0).Value);
        }


        public ILocalSave GetLocalSaveData()
        {
            JSONNode node = new JSONObject();

            node.Add(InventoryKeys.Local_Energy, Energy);
            node.Add(InventoryKeys.Local_Gold, Gold.ToString());

            return new LocalSaveJSON() { FilePath = FileUtils.ResolvePath(LOCAL_FILE_NAME), Node = node };
        }
    }
}
