﻿using Newtonsoft.Json;
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
        public IList<ImageVM> Images { get; set; } = new List<ImageVM>();

    }

    [JsonObject]
    public class ImageVM {

        [JsonProperty("origin")]
        public string OriginURL { get; set; }

        [JsonProperty("thumb")]
        public string ThumbnailURL { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("image")]
        public string Base64 { get; set; }

    }

}
