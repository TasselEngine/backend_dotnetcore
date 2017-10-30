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

namespace Tassel.API.Controllers {
    [Route("api/status")]
    public class StatusController : Controller {

        private IStatusService status;

        public StatusController(IStatusService status) {
            this.status = status;
        }

        [HttpGet("all")]
        public async Task<JsonResult> Get() {
            var (coll, status, error) = await this.status.GetCollectionAbstractAsync();
            if (status!=JsonStatus.Succeed)
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
            this.HttpContext.GetStringEntry(TokenClaimsKey.UUID, out var uuid);
            if (uuid == null)
                return this.JsonFormat(false, JsonStatus.UserNotLogin);
            if (uuid != vm.UserID)
                return this.JsonFormat(false, JsonStatus.UserNotMatched);
            this.HttpContext.GetStringEntry(TokenClaimsKey.Avatar, out var avatar);
            this.HttpContext.GetStringEntry(TokenClaimsKey.UserName, out var uname);
            if (vm.UserName == null)
                vm.UserName = uname;
            var (status, succeed, error) = await this.status.InsertOneAsync(ModelCreator.CreateStatus(vm, avatar));
            if (!succeed)
                return this.JsonFormat(false, JsonStatus.StatusInsertFailed, error.Read());
            return this.JsonFormat(true, content: status.ID);
        }

        [HttpPost("{id}/comment")]
        [Token, User]
        public async Task<JsonResult> AddCommentAsync(string id, [FromBody]CommentInsertVM vm) {
            if (vm == null)
                return this.JsonFormat(false, JsonStatus.BodyFormIsNull);
            this.HttpContext.GetStringEntry(TokenClaimsKey.UUID, out var uuid);
            if (uuid == null)
                return this.JsonFormat(false, JsonStatus.UserNotLogin);
            if (uuid != vm.UID)
                return this.JsonFormat(false, JsonStatus.UserNotMatched);
            this.HttpContext.GetStringEntry(TokenClaimsKey.Avatar, out var avatar);
            this.HttpContext.GetStringEntry(TokenClaimsKey.UserName, out var uname);
            if (vm.UName == null)
                vm.UName = uname;
            var model = default(Comment);
            var (status, error) = await this.status.AddCommentAsync(id, model = ModelCreator.CreateComment(vm, id, ModelType.Status, avatar));
            if (status != JsonStatus.Succeed)
                return this.JsonFormat(false, status, error.Read());
            return this.JsonFormat(true, content: model);
        }

        [HttpDelete("{id}/comment")]
        [Token, User]
        public async Task<JsonResult> DeleteCommentAsync(string id, string comt_id) {
            this.HttpContext.GetStringEntry(TokenClaimsKey.UUID, out var uuid);
            if (string.IsNullOrEmpty(comt_id))
                return this.JsonFormat(false, JsonStatus.QueryParamsNull);
            if (uuid == null)
                return this.JsonFormat(false, JsonStatus.UserNotLogin);
            var (status, error) = await this.status.RemoveCommentAsync(id, uuid, comt_id);
            if (status != JsonStatus.Succeed)
                return this.JsonFormat(false, status, error.Read());
            return this.JsonFormat(true);
        }

        [HttpPut("{id}/like")]
        [Token, User]
        public async Task<JsonResult> LikeAsync(string id, [FromBody]LikeVM vm) {
            if (vm == null)
                return this.JsonFormat(false, JsonStatus.BodyFormIsNull);
            this.HttpContext.GetStringEntry(TokenClaimsKey.UUID, out var uuid);
            if (uuid == null)
                return this.JsonFormat(false, JsonStatus.UserNotLogin);
            if (uuid != vm.UserID)
                return this.JsonFormat(false, JsonStatus.UserNotMatched);
            this.HttpContext.GetStringEntry(TokenClaimsKey.Avatar, out var avatar);
            this.HttpContext.GetStringEntry(TokenClaimsKey.UserName, out var uname);
            if (vm.UserName == null)
                vm.UserName = uname;
            var (user_id, status, error) = await this.status.LikeAsync(id, ModelCreator.CreateLike(vm, id, ModelType.Status, avatar));
            if (status != JsonStatus.Succeed)
                return this.JsonFormat(false, status, error.Read());
            return this.JsonFormat(true, content: user_id);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value) {
        }

        [HttpDelete("{id}")]
        public void Delete(int id) {
        }
    }
}
