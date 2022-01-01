using GM.Armoury.Models;
using GM.Artefacts.Models;
using GM.Bounties.Models;
using GM.BountyShop.Models;
using GM.Common.Interfaces;
using GM.Inventory.Models;
using GM.Mercs.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using GM.Common.Models;

namespace GM.HTTP.Requests
{
    public class UserLoginRequest : IServerRequest
    {
        public string DeviceId;

        public UserLoginRequest(string device)
        {
            DeviceId = device;
        }
    }


    public class UserLoginReponse : ServerResponse, IServerAuthentication
    {
        [JsonProperty(PropertyName = "sessionId", Required = Required.Always)]
        public string Session { get; set; }

        public ServerUserDataModel UserData { get; set; }
    }
}
