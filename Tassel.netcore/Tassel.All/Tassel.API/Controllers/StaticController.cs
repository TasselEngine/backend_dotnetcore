using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tassel.API.Utils.Authorization;
using Tassel.API.Utils.Extensions;
using Tassel.API.VM.File;
using Tassel.Services.Contract;

namespace Tassel.API.Controllers {

    [Route("api/static")]
    public class StaticController : Controller {

        private IStaticService srv;

        public StaticController(IStaticService srv) {
            this.srv = srv;
        }

        [HttpPost("image")]
        [Token, Admin]
        public async ValueTask<JsonResult> PushAsync([FromBody]UploadImageVM vm) {
            var (succeed, error, result) = await this.srv.CreateIamgeResourceAsync(vm.File);
            if (!succeed)
                return this.JsonFormat(false);
            return this.JsonFormat(true, content: result);
        }

        [HttpGet("tieba")]
        public JsonResult TiebaImagesGet() {
            var (_,_,images)=this.srv.GetTiebaImagesGroup();
            return this.JsonFormat(true, content: images);
        }

        [HttpGet("others")]
        public JsonResult OthersGet() {
            var (_, _, images) = this.srv.GetSinaOthersStickersGroup();
            return this.JsonFormat(true, content: images);
        }

        [HttpGet("sina_pop")]
        public JsonResult SinaPopGet() {
            var (_, _, images) = this.srv.GetSinaPopStickersGroup();
            return this.JsonFormat(true, content: images);
        }

        [HttpGet("sina_role")]
        public JsonResult SinaRolesGet() {
            var (_, _, images) = this.srv.GetSinaRoleStickersGroup();
            return this.JsonFormat(true, content: images);
        }

    }

}
