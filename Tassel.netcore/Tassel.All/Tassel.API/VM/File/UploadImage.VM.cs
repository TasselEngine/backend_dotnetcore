using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tassel.API.VM.File {

    [JsonObject]
    public class UploadImageVM {

        [JsonProperty("file")]
        public string File { get; set; }

    }

}
