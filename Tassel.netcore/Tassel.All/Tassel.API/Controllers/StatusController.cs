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

namespace Tassel.API.Controllers {
    [Route("api/status")]
    public class StatusController : Controller {

        private IStatusService status;

        public StatusController(IStatusService status) {
            this.status = status;
        }

        [HttpGet("all")]
        public async Task<JsonResult> Get() {
            var (coll, succeed, error) = await this.status.GetCollectionsAsync();
            if (!succeed)
                return this.JsonFormat(false, JsonStatus.StatusCollectionLoadFailed, error.Read());
            return this.JsonFormat(true, content: coll);
        }

        [HttpGet("{id}")]
        public async Task<JsonResult> Get(string id) {
            var (entry, status, error) = await this.status.GetStatusDetailsAsync(id);
            if(status == JsonStatus.Succeed)
                return this.JsonFormat(true, content: entry);
            return this.JsonFormat(false, status, error.Read());
        }

        [HttpPost("create")]
        [Token, Admin]
        public async Task<JsonResult> PostAsync() {
            // TEST
            var (parent, succeed, error) = await this.status.InsertOneAsync(new Status {
                Content = "hahahahahahahah",
                Creator = new BaseCreator { UUID = "4525224", UserName = "baba" },
                Images = new List<BaseImage> { new BaseImage { IsFile = false, Url = "http://p3.wmpic.me/article/2016/07/25/1469459240_PzFfSySK.jpg" } }
            });
            if (!succeed)
                return this.JsonFormat(false, JsonStatus.StatusInsertFailed, error.Read());
            return this.JsonFormat(true);
        }

        [HttpPost("{id}/comment")]
        [Token, User]
        public async Task<JsonResult> AddCommentAsync(string id, [FromBody]CommentInsertVM vm) {
            this.HttpContext.GetStringEntry(TokenClaimsKey.UUID, out var uuid);
            if (vm == null)
                return this.JsonFormat(false, JsonStatus.BodyIsNull);
            if (uuid == null)
                return this.JsonFormat(false, JsonStatus.UserNotLogin);
            if (uuid != vm.UID)
                return this.JsonFormat(false, JsonStatus.UserNotMatched);
            var (status, error) = await this.status.AddCommentAsync(id, new Comment {
                Creator = new BaseCreator { UserName = vm.UName, UUID = vm.UID },
                CommentContent = vm.Content,
                ParentID = id,
                ParentType = ModelType.Status
            });
            if (status != JsonStatus.Succeed)
                return this.JsonFormat(false, status, error.Read());
            return this.JsonFormat(true);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value) {
        }

        [HttpDelete("{id}")]
        public void Delete(int id) {
        }
    }
}
