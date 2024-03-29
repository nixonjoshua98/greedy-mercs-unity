﻿using GM.Models;
using Newtonsoft.Json;

namespace GM.HTTP.Requests
{
    public class LoginResponse : ServerResponse
    {
        [JsonProperty(Required = Required.Always)]
        public string Token { get; set; }

        public ServerUserDataModel UserData { get; set; }
    }
}
