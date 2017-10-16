using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tassel.API.VM.Status {

    [JsonObject]
    public class LikeVM {

        [JsonProperty("uid")]
        public string UserID { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

    }

}
