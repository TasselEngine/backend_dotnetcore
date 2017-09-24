using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Tassel.API.VM.Identity {
    [DataContract]
    public class UpdateUser {

        [DataMember(Name ="user")]
        public string UserName { get; set; }

        [DataMember(Name = "age")]
        public int Age { get; set; }

        [DataMember(Name = "f_name")]
        public string FamilyName { get; set; }

        [DataMember(Name = "g_name")]
        public string GivenName { get; set; }

    }
}
