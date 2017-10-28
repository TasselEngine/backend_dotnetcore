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
    [Token, Admin]
    public class StaticController : Controller {

        private IStaticService srv;

        public StaticController(IStaticService srv) {
            this.srv = srv;
        }

        [HttpPost("image")]
        public async ValueTask<JsonResult> PushAsync([FromBody]UploadImageVM vm) {
            var (succeed, error, result) = await this.srv.CreateIamgeResourceAsync(vm.File);
            if (!succeed)
                return this.JsonFormat(false);
            return this.JsonFormat(true, content: result);
        }

    }

}
