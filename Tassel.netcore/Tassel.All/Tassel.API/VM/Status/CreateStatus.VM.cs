using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tassel.API.VM.Status {

    [JsonObject]
    public class CreateStatusVM {

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("uid")]
        public string UserID { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("images")]
        public IList<string> Images { get; set; } = new List<string>();

    }

}
