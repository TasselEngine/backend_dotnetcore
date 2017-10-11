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
        private ICommentService comments;
        private ILikesService likes;

        public StatusController(IStatusService status, ICommentService coms, ILikesService likes) {
            this.status = status;
            this.comments = coms;
            this.likes = likes;
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
            var (coll, succ02, error02) = await this.comments.GetCollectionsAsync(i => i.ParentID == entry.ID && i.ParentType == ModelType.Status);
            if (succ02)
                entry.Comments = coll;
            var (likers, succ03, error03) = await this.likes.GetCollectionsAsync(i => i.ParentID == entry.ID && i.TargetType == ModelType.Status);
            if (succ03)
                entry.Likes = likers;
            return this.JsonFormat(true, content: entry);
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
            var (_, succ02, err02) = await this.comments.InsertOneAsync(new Comment {
                CommentContent = "I want to add a comment.",
                ParentID = parent.ID,
                ParentType = ModelType.Status,
                Creator = new BaseCreator { UUID = "234125", UserName = "Bruce Wayne" }
            });
            if (!succ02)
                return this.JsonFormat(false, JsonStatus.CommentAddFailed, err02.Read());
            var (_, succ03, err03) = await this.likes.InsertOneAsync(new LikesEntry {
                User = new BaseCreator { UUID = "4525224", UserName = "baba" },
                ParentID = parent.ID,
                TargetType = ModelType.Status
            });
            if (!succ03)
                return this.JsonFormat(false, JsonStatus.LikesAddFailed, err03.Read());
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
