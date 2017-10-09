using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Tassel.Model.Models.BsonModels;
using Tassel.Model.Utils;

namespace Tassel.Model.Models.BsonModels {

    [JsonObject]
    public class Status : BasePageEntry {

        public override ModelType Type { get; } = ModelType.Status;

    }

}
