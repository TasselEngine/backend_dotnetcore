using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Utils;

namespace Tassel.Services.Contract {

    [JsonObject]
    public class ImageResult {

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("origin")]
        public string OriginImagePath { get; set; }

        [JsonProperty("thumb")]
        public string ThumbnailPath { get; set; }

    }

    public interface IStaticService {

        ValueTask<(bool succeed, Error error, ImageResult result)> CreateIamgeResourceAsync(string base64FileString);

        ValueTask<(bool succeed, Error error)> DeleteIamgeResourceAsync(string base64FileString);

        (bool succeed, Error error, IList<KeyValuePair<string, string>> images) GetTiebaImagesGroup();

    }
}
