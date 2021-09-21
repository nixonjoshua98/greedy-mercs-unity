// For now we will lazy instantiate but we will most likely need to move to a controlled creation

namespace GM.Core
{
    public class GMApplication : Common.MonoBehaviourSingleton<GMApplication>
    {
        public UserData PlayerData => UserData.Get;
        public GameData GameData => GameData.Get;
    }
}