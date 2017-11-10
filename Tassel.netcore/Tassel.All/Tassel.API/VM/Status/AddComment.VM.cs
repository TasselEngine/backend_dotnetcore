﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tassel.API.VM.Status {

    [JsonObject]
    public class CommentAddVM {

        [JsonProperty("is_reply")]
        public bool IsReply { get; set; } = false;

        [JsonProperty("com_id")]
        public string CommentID { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("user_name")]
        public string UName { get; set; }

        [JsonProperty("uid")]
        public string UID { get; set; }

        [JsonProperty("mend_user")]
        public string MName { get; set; }

        [JsonProperty("m_uid")]
        public string MUID { get; set; }

    }

}