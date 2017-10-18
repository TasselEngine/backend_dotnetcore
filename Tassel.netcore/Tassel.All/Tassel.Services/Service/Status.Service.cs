using BWS.Utils.NetCore.Expressions;
using BWS.Utils.NetCore.Format;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Model.Utils;
using Tassel.Services.Contract;
using Tassel.Services.Contract.Providers;

namespace Tassel.Services.Service {

    public class StatusService : DeleteSafeBase<Status>, IStatusService {

        private ICommentServiceProvider<Comment> comments;
        private ILikesServiceProvider likes;

        public ICommentServiceProvider<Comment> Comments => this.comments;

        public ILikesServiceProvider Likes => this.likes;

        public StatusService(
            MongoDBContext db,
            ICommentServiceProvider<Comment> coms,
            ILikesServiceProvider likes) : base(db, ModelCollectionName.Status) {
            this.comments = coms;
            this.likes = likes;
        }

        protected override UpdateDefinition<Status> DefineUpdate(Status entry) {
            var def = base.DefineUpdate(entry);
            if (entry == null)
                return def;
            if (entry.Cover != null)
                def = def.Set(i => i.Cover, entry.Cover);
            if (entry.Content != null)
                def = def.Set(i => i.Content, entry.Content);
            return def;
        }

        protected UpdateDefinition<Status> DefineCommentsUpdate(string comment_id, bool add = true) {
            var def = Builders<Status>.Update;
            return add ?
                def.Push(i => i.CommentIDs, comment_id) :
                def.Pull(i => i.CommentIDs, comment_id);
        }

        protected UpdateDefinition<Status> DefineLikersUpdate(string user_uuid, bool add = true) {
            var def = Builders<Status>.Update;
            return add ?
                def.AddToSet(i => i.LikerIDs, user_uuid) :
                def.Pull(i => i.LikerIDs, user_uuid);
        }

        protected UpdateDefinition<Status> DefineImagesUpdate(BaseImage image, bool add = true) {
            var def = Builders<Status>.Update.Set(i => i.UpdateTime, DateTime.UtcNow.ToUnix());
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
            var (coll, succ02, _) = await this.comments.GetCollectionsAsync(i => i.ParentID == entry.ID && i.ParentType == ModelType.Status);
            if (succ02) 
                entry.Comments = coll;
            var (likers, succ03, _) = await this.likes.GetCollectionsAsync(i => i.ParentID == entry.ID && i.TargetType == ModelType.Status);
            if (succ03) 
                entry.Likes = likers;
            return (entry, JsonStatus.Succeed, Error.Empty);
        }

        public async ValueTask<(JsonStatus status, Error error)> AddCommentAsync(string id, Comment comment) {
            var (entry, succeed, error) = await this.FindOneUpdateAsync(id, null, this.DefineCommentsUpdate(comment.ID));
            if(!succeed)
                return (JsonStatus.CommentAddFailed, error);
            if(entry==null)
                return (JsonStatus.CommentAddFailed, Error.Create(Errors.EntryNotExist));
            (_, succeed, error) = await this.comments.InsertOneAsync(comment);
            if (!succeed) {
                // Rollback, anyway
                await this.FindOneUpdateAsync(id, null, this.DefineCommentsUpdate(comment.ID, false));
                return (JsonStatus.CommentAddFailed, error);
            }
            return (JsonStatus.Succeed, Error.Empty);
        }

        public async ValueTask<(JsonStatus status, Error error)> RemoveCommentAsync(string id, string uid, string comment_id) {
            var (entry, succeed, error) = await this.FindOneUpdateAsync(id, null, this.DefineCommentsUpdate(comment_id, false));
            if (!succeed)
                return (JsonStatus.CommentRemoveFailed, error);
            if (entry == null)
                return (JsonStatus.CommentRemoveFailed, Error.Create(Errors.EntryNotExist));
            (succeed, error) = await this.comments.DeleteOneByFilterAsync(i => i.ID == comment_id && i.Creator.UUID == uid && i.ParentID == id);
            if (!succeed) {
                // Rollback, anyway
                await this.FindOneUpdateAsync(id, null, this.DefineCommentsUpdate(comment_id));
                return (JsonStatus.CommentAddFailed, error);
            }
            return (JsonStatus.Succeed, Error.Empty);
        }

        public async ValueTask<(string user_id, JsonStatus status, Error error)> LikeAsync(string id, LikesEntry like) {
            var (entry, succeed, error) = await this.FindOneByIDAsync(id);
            if (!succeed)
                return (default(string), JsonStatus.StatusNotFound, error);
            if (entry.LikerIDs.Contains(like.User.UUID) ||entry.Likes.FirstOrDefault(i=>i.User.UUID== like.User.UUID) !=null) {
                (succeed, error) = await this.likes.DeleteAllByIDsAsync(id, like.User.UUID);
                if (!succeed)
                    return (default(string), JsonStatus.LikesRemoveFailed, error);
                (_, succeed, error) = await this.UpdateOneAsync(id, null, this.DefineLikersUpdate(like.User.UUID, false));
                if(!succeed)
                    return (default(string),JsonStatus.LikesRemoveFailed, error);
                return ("deleted", JsonStatus.Succeed, Error.Empty);
            } else {
                (_, succeed, error) = await this.likes.InsertOneAsync(like);
                if (!succeed)
                    return (default(string),JsonStatus.LikesAddFailed, error);
                (_, succeed, error) = await this.UpdateOneAsync(id, null, this.DefineLikersUpdate(like.User.UUID));
                if (!succeed)
                    return (default(string), JsonStatus.LikesAddFailed, error);
                return (like.User.UUID, JsonStatus.Succeed, Error.Empty);
            }
        }

    }

}
