using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tassel.API.VM.Status {

    [JsonObject]
    public class CommentInsertVM {

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("uname")]
        public string UName { get; set; }

        [JsonProperty("uid")]
        public string UID { get; set; }

    }

}
