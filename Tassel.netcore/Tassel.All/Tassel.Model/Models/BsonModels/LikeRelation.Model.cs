﻿using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Tassel.Model.Utils;

namespace Tassel.Model.Models.BsonModels {

    public interface ILikeable : IBaseModel{
        string ID { get; set; }
        ModelType TargetType { get; set; }
        string ParentID { get; set; }
        IBaseCreator User { get; set; }
    }

    [JsonObject]
    public class LikesEntry : BaseModel , ILikeable {

        [JsonIgnore]
        public override string ID { get; set; } = IdentityProvider.CreateGuid(GuidType.N);

        public override ModelType Type { get; } = ModelType.LikeEntry;

        [BsonElement("target_type")]
        [JsonIgnore]
        public ModelType TargetType { get; set; } = ModelType.Default;

        [BsonElement("parent_id")]
        [JsonIgnore]
        public string ParentID { get; set; }

        [BsonElement("user")]
        [JsonProperty("user")]
        public IBaseCreator User { get; set; }

        [JsonIgnore]
        public override long? UpdateTime { get; set; }

    }

}
