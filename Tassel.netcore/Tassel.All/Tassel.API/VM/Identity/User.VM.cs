using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Tassel.API.VM.Identity {

    [DataContract]
    public class UserVM {

        [DataMember(Name ="user")]
        public dynamic User { get; set; }

        [DataMember(Name = "more")]
        public dynamic More { get; set; }

    }
}
