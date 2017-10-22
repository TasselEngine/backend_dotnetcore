using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Utils;

namespace Tassel.Services.Contract {

    [JsonObject]
    public class ImageResult {

        [JsonProperty("origin")]
        public string OriginImagePath { get; set; }

        [JsonProperty("large")]
        public string LargeImagePath { get; set; }

        [JsonProperty("normal")]
        public string MiddleImagePath { get; set; }

        [JsonProperty("thumb")]
        public string ThumbnailPath { get; set; }

    }

    public interface IStaticService {

        ValueTask<(bool succeed, Error error, ImageResult result)> CreateIamgeResourceAsync(string base64FileString);

        ValueTask<(bool succeed, Error error)> DeleteIamgeResourceAsync(string base64FileString);

    }
}
