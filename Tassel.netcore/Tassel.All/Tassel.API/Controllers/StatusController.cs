using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tassel.Model.Models.BsonModels;
using Tassel.API.Utils.Extensions;
using Tassel.Model.Models;
using Tassel.Services.Contract;
using Tassel.API.VM.Status;
using Tassel.API.Utils.Authorization;
using Tassel.Services.Utils.Constants;
using Tassel.API.VM;
using BWS.Utils.NetCore.Format;

namespace Tassel.API.Controllers {
    [Route("status")]
    public class StatusController : Controller {

        private IStatusService status;
        private ILogService logger;
        private IMessageService message;

        public StatusController(IStatusService status, ILogService logsrv, IMessageService msgSrv) {
            this.status = status;
            this.logger = logsrv;
            this.message = msgSrv;
        }

        [HttpGet("all")]
        public async Task<JsonResult> Get() {
            var (coll, status, error) = await this.status.GetCollectionAbstractAsync(DateTime.UtcNow.ToUnix(), null);
            if (status != JsonStatus.Succeed)
                return this.JsonFormat(false, status, error.Read());
            return this.JsonFormat(true, content: coll);
        }

        [HttpGet("gets")]
        public async Task<JsonResult> Gets(long? before, int? take) {
            var (coll, status, error) = await this.status.GetCollectionAbstractAsync(before, take);
            if (status != JsonStatus.Succeed)
                return this.JsonFormat(false, status, error.Read());
            return this.JsonFormat(true, content: coll);
        }

        [HttpGet("{id}")]
        public async Task<JsonResult> Get(string id) {
            var (entry, status, error) = await this.status.GetStatusDetailsAsync(id);
            if (status == JsonStatus.Succeed)
                return this.JsonFormat(true, content: entry);
            return this.JsonFormat(false, status, error.Read());
        }

        [HttpPost("create")]
        [Token, Admin]
        public async Task<JsonResult> PostAsync([FromBody]CreateStatusVM vm) {

            if (vm == null)
                return this.JsonFormat(false, JsonStatus.BodyFormIsNull);

            var user = this.GetUser();
            if (user.UUID == null)
                return this.JsonFormat(false, JsonStatus.UserNotLogin);
            if (user.UUID != vm.UserID)
                return this.JsonFormat(false, JsonStatus.UserNotMatched);

            if (vm.UserName == null)
                vm.UserName = user.UserName;

            var md = default(Status);
            var (status, succeed, error) = await this.status.InsertOneAsync(md = ModelCreator.CreateStatus(vm, user.AvatarUrl));
            if (!succeed)
                return this.JsonFormat(false, JsonStatus.StatusInsertFailed, error.Read());

            await this.logger.Info(
                new Agent(user.UUID, user.UserName, user.AvatarUrl),
                new Target(md.ID, vm.Content, $"image_count : {vm.Images.Count}"),
                ModelType.Status,
                LogAction.Insert,
                this.GetRole());

            return this.JsonFormat(true, content: status.ID);
        }

        [HttpPost("{id}/comment")]
        [Token, User]
        public async Task<JsonResult> AddCommentAsync(string id, [FromBody]CommentAddVM vm) {

            if (vm == null)
                return this.JsonFormat(false, JsonStatus.BodyFormIsNull);

            var creator = this.GetUser();
            if (creator.UUID == null)
                return this.JsonFormat(false, JsonStatus.UserNotLogin);
            if (creator.UUID != vm.UID)
                return this.JsonFormat(false, JsonStatus.UserNotMatched);
            if (vm.UName == null)
                vm.UName = creator.UserName;

            var model = default(Comment);
            var (status, error) = default((JsonStatus, Model.Utils.Error));
            if (vm.IsReply) {
                (status, error) = await this.status.Comments.AddReplyForCommentAsync(
                    vm.CommentID, model = ModelCreator.CreateComment(vm, vm.CommentID, ModelType.Comment, creator.AvatarUrl));
            } else {
                (status, error) = await this.status.AddCommentAsync(
                    id, model = ModelCreator.CreateComment(vm, id, ModelType.Status, creator.AvatarUrl));
            }
            if (status != JsonStatus.Succeed)
                return this.JsonFormat(false, status, error.Read());

            // send message
            var (entry, status02, error02) = await this.status.GetStatusDetailsAsync(id);
            var source = new MessageSource {
                Type = ModelType.Comment,
                HostID = id,
                TargetID = model.ID,
                HostType = ModelType.Status,
                HostAbstract = status02 == JsonStatus.Succeed ? entry.Content : null,
                Abstract = model.CommentContent
            };
            if (vm.IsReply) {
                await this.message.CreateMessageAsync(creator, model.Creator, MessageType.Reply, null, source);
            } else {
                await this.message.CreateMessageAsync(creator, model.Creator, MessageType.Comment, null, source);
            }

            return this.JsonFormat(true, content: model);
        }

        [HttpDelete("{id}/comment")]
        [Token, User]
        public async Task<JsonResult> DeleteCommentAsync(string id, string comt_id, bool is_reply = false, string reply_id = null) {

            if (string.IsNullOrEmpty(comt_id) || string.IsNullOrEmpty(id))
                return this.JsonFormat(false, JsonStatus.QueryParamsNull);

            var user = this.GetUser();
            if (user.UUID == null)
                return this.JsonFormat(false, JsonStatus.UserNotLogin);

            var (status, error) = default((JsonStatus, Model.Utils.Error));
            if (is_reply) {
                if (reply_id == null)
                    return this.JsonFormat(false, JsonStatus.CommentRemoveFailed);
                (status, error) = await this.status.Comments.RemoveReplyForCommentAsync(comt_id, reply_id, user.UUID);
            } else {
                (status, error) = await this.status.RemoveCommentAsync(id, user.UUID, comt_id);
            }
            if (status != JsonStatus.Succeed)
                return this.JsonFormat(false, status, error.Read());

            return this.JsonFormat(true);
        }

        [HttpPut("{id}/like")]
        [Token, User]
        public async Task<JsonResult> LikeAsync(string id, [FromBody]LikeVM vm) {

            if (vm == null)
                return this.JsonFormat(false, JsonStatus.BodyFormIsNull);
            var user = this.GetUser();
            if (user.UUID == null)
                return this.JsonFormat(false, JsonStatus.UserNotLogin);
            if (user.UUID != vm.UserID)
                return this.JsonFormat(false, JsonStatus.UserNotMatched);

            if (vm.UserName == null)
                vm.UserName = user.UserName;

            var (user_id, status, error) = await this.status.LikeAsync(id, ModelCreator.CreateLike(vm, id, ModelType.Status, user.AvatarUrl));
            if (status != JsonStatus.Succeed)
                return this.JsonFormat(false, status, error.Read());

            // send message
            if (user_id != "deleted") {
                var (entry, status02, error02) = await this.status.GetStatusDetailsAsync(id);
                var source = new MessageSource {
                    Type = ModelType.Status,
                    HostID = id,
                    TargetID = id,
                    HostType = ModelType.Status,
                    HostAbstract = status02 == JsonStatus.Succeed ? entry.Content : null,
                    Abstract = entry.Content
                };
                await this.message.CreateMessageAsync(user, entry.Creator, MessageType.Like, null, source);
            }

            return this.JsonFormat(true, content: user_id);
        }

        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value) {
        //}

        [HttpDelete("{id}")]
        [Token, Admin]
        public async Task<JsonResult> DeleteAsync(string id) {
            if (id == null)
                return this.JsonFormat(false, JsonStatus.QueryParamsNull);

            var user = this.GetUser();
            var role = this.GetRole();

            var (status, error) = await this.status.DeleteStatusAsync(id);
            if (status != JsonStatus.Succeed)
                return this.JsonFormat(false, status, error.Read());

            await this.logger.Warn(
                new Agent(user.UUID, user.UserName, user.AvatarUrl),
                new Target(id, null, $"delete status."),
                ModelType.Status,
                LogAction.Delete,
                role);

            return this.JsonFormat(true, content: id);
        }
    }
}
