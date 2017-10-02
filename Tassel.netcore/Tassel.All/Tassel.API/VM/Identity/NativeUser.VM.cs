using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tassel.API.VM.Identity {

    [JsonObject]
    public class NativeUser {

        [JsonProperty("user")]
        public string UserName { get; set; }

        [JsonProperty("psd")]
        public string Password { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

    }

}
