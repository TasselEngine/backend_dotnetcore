using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tassel.Model.Models;
using MongoDB.Driver;
using Tassel.API.Utils.Extensions;

namespace Tassel.API.Controllers {
    [Route("api/status")]
    public class StatusController : Controller {

        private IMongoDatabase mdb;
        private IMongoCollection<Status> status;

        public StatusController(MongoDBContext mongo) {
            this.mdb = mongo.DB;
            this.status = this.mdb.GetCollection<Status>("status");
        }

        [HttpGet("all")]
        public JsonResult Get() {
            return this.JsonFormat(true, content: this.status.AsQueryable().Where(i => true));
        }

        [HttpGet("{id}")]
        public string Get(int id) {
            return "value";
        }

        [HttpPost("create")]
        public async Task<JsonResult> PostAsync() {
            await this.status.InsertOneAsync(new Status { Content = "abcdefg", Creator = new StatusCreator { UUID = "sadwarb", UserName = "miao17game" } });
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
