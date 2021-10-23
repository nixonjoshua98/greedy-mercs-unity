using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.HTTP
{
    public interface IServerAuthentication
    {
        public string UserId { get; set; }
        public string SessionId { get; set; }
    }
}
