using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSD_Project_CC2004V_Team08.Models
{
    [JsonObject(MemberSerialization.OptOut)]

    public class TokenResponseViewModel
    {
        public TokenResponseViewModel()
        {

        }

        public string token { get; set; }
        public int expiration { get; set; }
    }
}
