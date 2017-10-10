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
            if (!succeed)
                return this.JsonFormat(false, JsonStatus.StatusCollectionLoadFailed, error.Read());
            return this.JsonFormat(true, content: coll);
        }

        [HttpGet("{id}")]
        public async Task<JsonResult> Get(string id) {
            var (entry, succeed, error) = await this.status.FindOneByIDAsync(id);
            if(!succeed)
                return this.JsonFormat(false, JsonStatus.Error, error.Read());
            if (entry == null)
                return this.JsonFormat(false, JsonStatus.StatusNotFound);
            return this.JsonFormat(true, content: entry);
        }

        [HttpPost("create")]
        public async Task<JsonResult> PostAsync() {
            // TEST
            var (_, succeed, error) = await this.status.InsertOneAsync(new Status {
                Content = "hahahahahahahah",
                Creator = new BaseCreator { UUID = "4525224", UserName = "baba" }
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
