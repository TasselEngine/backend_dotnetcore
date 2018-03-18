using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tassel.API.VM.Status {

    [JsonObject]
    public class CommentAddVM {

        [JsonProperty("is_reply")]
        public bool IsReply { get; set; } = false;

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("user_name")]
        public string UName { get; set; }

        [JsonProperty("uid")]
        public string UID { get; set; }

        [JsonProperty("reply_content")]
        public ReplyContent MContent { get; set; }

    }

    [JsonObject]
    public class ReplyContent {

        [JsonProperty("com_id")]
        public string CommentID { get; set; }

        [JsonProperty("mend_user")]
        public string UserName { get; set; }

        [JsonProperty("m_uid")]
        public string UUID { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

    }

}
