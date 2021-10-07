using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.HTTP.Requests
{
    public class UserLoginRequest : IServerRequest
    {
        public string DeviceId;
    }


    public class UserLoginReponse : ServerResponse
    {

    }
}
