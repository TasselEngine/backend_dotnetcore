using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Tassel.Model.Utils;

namespace Tassel.Model.Models.BsonModels {

    public enum ReadType {
        Unread = 0,
        Read = 1
    }

    public enum MessageType {
        Default = 0,
        Reply = 1,
        Comment = 2,
        Like = 3,
        Methioned = 4,
        Repost = 5,
        Follow = 6,
        Chat = 7,
    }

    public interface IMessage<T> : IBaseCreateModel<BaseCreator> where T : IMessageSource {
        MessageType MessageType { get; set; }
        ReadType ReadState { get; set; }
        BaseCreator Receiver { get; set; }
        string Content { get; set; }
        T Source { get; set; }
    }

    [JsonObject]
    public class Message : BaseCreateModel, IMessage<MessageSource> {

        public override ModelType Type { get; } = ModelType.Message;

        [BsonElement("read_state")]
        [JsonProperty("read_state")]
        public ReadType ReadState { get; set; } = ReadType.Unread;

        [BsonElement("msg_type")]
        [JsonProperty("msg_type")]
        public MessageType MessageType { get; set; } = MessageType.Default;

        [BsonElement("target")]
        [JsonProperty("user")]
        public BaseCreator Receiver { get; set; }

        [BsonElement("content")]
        [JsonProperty("content")]
        public string Content { get; set; }

        [BsonElement("source")]
        [JsonProperty("source")]
        public MessageSource Source { get; set; }

    }

    public interface IMessageSource{
        string TargetID { get; set; }
        string HostID { get; set; }
        ModelType Type { get; set; }
        ModelType HostType { get; set; }
        string Abstract { get; set; }
        string HostAbstract { get; set; }
}

    [JsonObject]
    public class MessageSource : IMessageSource {

        [BsonElement("type")]
        [JsonProperty("type")]
        public ModelType Type { get; set;  } = ModelType.Default;

        [BsonElement("tid")]
        [JsonProperty("tid")]
        public string TargetID { get; set; }

        [BsonElement("abst")]
        [JsonProperty("abst")]
        public string Abstract { get; set; }

        [BsonElement("host_type")]
        [JsonProperty("host_type")]
        public ModelType HostType { get; set;  } = ModelType.Default;

        [BsonElement("host_id")]
        [JsonProperty("host_id")]
        public string HostID { get; set; }

        [BsonElement("host_abst")]
        [JsonProperty("host_abst")]
        public string HostAbstract { get; set; }

    }
}
