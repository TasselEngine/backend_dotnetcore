using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tassel.Model.Models.BsonModels;
using MongoDB.Driver;
using Tassel.API.Utils.Extensions;
using Tassel.Model.Models;
using Tassel.Services.Service;
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
            if(!succeed)
                return this.JsonFormat(false, JsonStatus.StatusCollectionLoadFailed, error.Read());
            return this.JsonFormat(true, content: coll);
        }

        [HttpGet("{id}")]
        public string Get(int id) {
            return "value";
        }

        [HttpPost("create")]
        public async Task<JsonResult> PostAsync() {
            // TEST
            var (_, succeed, error) = await this.status.InsertOneAsync(new Status {
                Content = "hahahahahahahah",
                Creator = new BaseCreator { UUID = "4525224", UserName = "baba" },
                Likes = new List<BaseCreator> {
                   new BaseCreator { UUID = "4525224", UserName = "baba" },
                   new BaseCreator { UUID = "3452344", UserName = "hehe" },
                },
                Comments = new List<BaseComment> {
                    new BaseComment { Content = "6666666666", Creator = new BaseCreator { UUID = "4525224", UserName = "baba" } },
                    new BaseComment { Content = "2333333333", Creator = new BaseCreator { UUID = "3452344", UserName = "hehe" } },
                }
            });
            if(!succeed)
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
