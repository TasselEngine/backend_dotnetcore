using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tassel.Model.Models.BsonModels;
using Tassel.API.Utils.Extensions;
using Tassel.Model.Models;
using Tassel.Services.Contract;

namespace Tassel.API.Controllers {
    [Route("api/status")]
    public class StatusController : Controller {

        private IStatusService status;

        public StatusController(IStatusService status) {
            this.status = status;
        }

        [HttpGet("all")]
        public JsonResult Get() {
            var (coll, succeed, error) = this.status.GetCollections();
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

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value) {
        }

        [HttpDelete("{id}")]
        public void Delete(int id) {
        }
    }
}
