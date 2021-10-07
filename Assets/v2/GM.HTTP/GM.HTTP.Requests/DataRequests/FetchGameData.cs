using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.HTTP.Requests
{
    public class FetchGameDataRequest : AuthorisedRequest
    {

    }


    public class FetchGameDataResponse : ServerResponse, Common.ICompleteGameData
    {

    }
}
