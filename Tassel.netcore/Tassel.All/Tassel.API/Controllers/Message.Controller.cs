using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tassel.API.Utils.Authorization;
using Tassel.API.Utils.Extensions;
using Tassel.API.VM.Message;
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
        public async ValueTask<JsonResult> FetchMessagesAsync(int count = 20, long? before = null, long? after = null, bool? unread = null) {
            this.HttpContext.GetStringEntry(TokenClaimsKey.UUID, out var uuid);
            if (uuid == null)
                return this.JsonFormat(false, JsonStatus.UserNotLogin);
            var (collection, status, error) = await this.srv.FetchMessagesAsync(uuid, before, after, count, unread);
            if (status != JsonStatus.Succeed)
                return this.JsonFormat(false, status, error.Read());
            return this.JsonFormat(true, content: collection);
        }

        [HttpPut("read")]
        public async ValueTask<JsonResult> ReadMessagesAsync([FromBody]ReadMessageVM vm) {
            if (vm == null)
                return this.JsonFormat(false, JsonStatus.BodyFormIsNull);
            var ids = vm.Messages ?? new string[0];
            var (count, statuc, error) = await this.srv.ReadMessagesAsync(ids);
            if (statuc != JsonStatus.Succeed)
                return this.JsonFormat(false, JsonStatus.ReadMessageFailed, error.Read());
            return this.JsonFormat(true, content: count);
        }

    }

}
