using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Tassel.API.VM.Token {

    [DataContract]
    public class TokenUserDetailsVM {

        [DataMember(Name = "user")]
        public dynamic User { get; set; }

        [DataMember(Name = "more")]
        public ThirdPartUserInfosVM More { get; set; }

    }

    [DataContract]
    public class ThirdPartUserInfosVM {

        [DataMember(Name = "weibo")]
        public dynamic Weibo { get; set; }

        [DataMember(Name = "wechat")]
        public dynamic Wechat { get; set; }

        [DataMember(Name = "qq")]
        public dynamic QQ { get; set; }

    }

    [DataContract]
    public class TokenProviderVM {

        [DataMember(Name = "token")]
        public string Token { get; set; }

        [DataMember(Name = "expires")]
        public int Expires { get; set; }

        [DataMember(Name = "details")]
        public TokenUserDetailsVM Details { get; set; }

    }
}
