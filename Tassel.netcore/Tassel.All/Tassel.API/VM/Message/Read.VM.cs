using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tassel.API.VM.Message {

    [JsonObject]
    public class ReadMessageVM {

        [JsonProperty("messages")]
        public string[] Messages { get; set; }

    }

}
