using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tassel.API.Utils.Authorization;
using Tassel.API.Utils.Extensions;
using Tassel.API.VM.File;
using Tassel.Model.Models;
using Tassel.Services.Contract;
using Tassel.Services.Utils.Constants;

namespace Tassel.API.Controllers {

    [Route("message")]
    [Token]
    public class MessageController : Controller {

        private IMessageService srv;

        public MessageController(IMessageService srv) {
            this.srv = srv;
        }

        [HttpGet("fetch")]
        public async ValueTask<JsonResult> FetchMessagesAsync(int count = 20, long? before = null, bool? unread = null) {
            this.HttpContext.GetStringEntry(TokenClaimsKey.UUID, out var uuid);
            if (uuid == null)
                return this.JsonFormat(false, JsonStatus.UserNotLogin);
            System.Diagnostics.Debug.WriteLine($"[{uuid}]-[count:{count}]-[before:{before}]-[unread:{unread}]");
            var (collection, status, error) = await this.srv.FetchMessagesAsync(uuid, before, count, unread);
            if (status != JsonStatus.Succeed)
                return this.JsonFormat(false, status, error.Read());
            return this.JsonFormat(true, content: collection);
        }

    }

}
