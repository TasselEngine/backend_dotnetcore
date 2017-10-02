using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Tassel.API.VM.Identity;

namespace Tassel.API.VM.Token {

    [DataContract]
    public class TokenProviderVM {

        [DataMember(Name = "token")]
        public string Token { get; set; }

        [DataMember(Name = "expires")]
        public int Expires { get; set; }

        [DataMember(Name = "user")]
        public DynamicUser Details { get; set; }

    }

    [DataContract]
    public class JwtProviderParam {

        [DataMember(Name = "user")]
        public string UserName { get; set; }

        [DataMember(Name = "psd")]
        public string Password { get; set; }

        [DataMember(Name = "wuid")]
        public string WeiboUID { get; set; }
    }

}
