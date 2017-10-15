using BWS.Utils.NetCore.Format;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Model.Utils;
using Tassel.Services.Contract;
using Tassel.Services.Contract.Providers;

namespace Tassel.Services.Service {

    public class StatusService : BaseService<Status>, IStatusService {

        private ICommentServiceProvider<Comment> comments;
        private ILikesServiceProvider likes;

        public ICommentServiceProvider<Comment> Comments => this.comments;

        public ILikesServiceProvider Likes => this.likes;

        public StatusService(
            MongoDBContext db, 
            ICommentServiceProvider<Comment> coms, 
            ILikesServiceProvider likes) :base(db, ModelCollectionName.Status) {
            this.comments = coms;
            this.likes = likes;
        }

        public override UpdateDefinition<Status> DefineUpdate(Status entry) {
            var def = base.DefineUpdate(entry);
            if (entry == null)
                return def;
            if (entry.Cover != null)
                def = def.Set(i => i.Cover, entry.Cover);
            if (entry.Content != null)
                def = def.Set(i => i.Content, entry.Content);
            return def;
        }

        public UpdateDefinition<Status> DefineCommentsUpdate(string comment_id, bool add = true) {
            var def = Builders<Status>.Update;
            return add? 
                def.Push(i => i.CommentIDs, comment_id): 
                def.Pull(i=>i.CommentIDs, comment_id);
        }

        public UpdateDefinition<Status> DefineLikersUpdate(string likers_id, bool add = true) {
            var def = Builders<Status>.Update;
            return add? 
                def.Push(i => i.CommentIDs, likers_id): 
                def.Pull(i => i.CommentIDs, likers_id);
        }

        public UpdateDefinition<Status> DefineImagesUpdate(BaseImage image, bool add = true) {
            var def = Builders<Status>.Update.Set(i=>i.UpdateTime, DateTime.UtcNow.ToUnix());
            return add ? 
                def.Push(i => i.Images, image) : 
                def.Pull(i => i.Images, image);
        }

        public async ValueTask<(Status entry, JsonStatus status, Error error)> GetStatusAbstractAsync(string id) {
            var (entry, succeed, error) = await this.FindOneByIDAsync(id);
            if (!succeed)
                return (default(Status), JsonStatus.Error, error);
            if (entry == null)
                return (default(Status), JsonStatus.StatusNotFound, error);
            return (entry, JsonStatus.Succeed, Error.Empty);
        }

        public async ValueTask<(Status entry, JsonStatus status, Error error)> GetStatusDetailsAsync(string id) {
            var (entry, status, error) = await this.GetStatusAbstractAsync(id);
            if (entry == null)
                return (entry, status, error);
            var (coll, succ02, error02) = await this.comments.GetCollectionsAsync(i => i.ParentID == entry.ID && i.ParentType == ModelType.Status);
            if (succ02)
                entry.Comments = coll;
            var (likers, succ03, error03) = await this.likes.GetCollectionsAsync(i => i.ParentID == entry.ID && i.TargetType == ModelType.Status);
            if (succ03)
                entry.Likes = likers;
            return (entry, JsonStatus.Succeed, Error.Empty);
        }

        public async ValueTask<(JsonStatus status, Error error)> AddCommentAsync(string id, Comment comment) {
            //var (entry, succeed, error) = await this.FindOneByIDAsync(id);
            //if (!succeed)
            //    return (JsonStatus.StatusNotFound, Error.Create(Errors.QueryEntryFailed));
            try {
                await this.comments.InsertOneAsync(comment);
                await this.UpdateOneAsync(null, id, this.DefineCommentsUpdate(comment.ID));
                return (JsonStatus.Succeed, Error.Empty);
            } catch(Exception e) {
                return (JsonStatus.CommentAddFailed, Error.Create(Errors.InsertOneFailed, e.Message));
            }
        }

        public async ValueTask<(JsonStatus status, Error error)> RemoveCommentAsync(string id, string comment_id) {
            //var (entry, succeed, error) = await this.FindOneByIDAsync(id);
            //if (!succeed)
            //    return (JsonStatus.StatusNotFound, Error.Create(Errors.QueryEntryFailed));
            try {
                await this.comments.DeleteOneAsync(comment_id);
                await this.UpdateOneAsync(null, id, this.DefineCommentsUpdate(comment_id, false));
                return (JsonStatus.Succeed, Error.Empty);
            } catch (Exception e) {
                return (JsonStatus.CommentRemoveFailed, Error.Create(Errors.DeleteEntryFailed, e.Message));
            }
        }
    }

}
