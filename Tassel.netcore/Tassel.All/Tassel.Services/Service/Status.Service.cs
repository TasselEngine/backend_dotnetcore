using BWS.Utils.NetCore.Format;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Model.Utils;
using Tassel.Services.Components;
using Tassel.Services.Contract;
using Tassel.Services.Contract.Components;
using Tassel.Services.Contract.Providers;

namespace Tassel.Services.Service {

    public class StatusService : LogicallyDeleteBase<Status>, IStatusService {

        private ICommentComponent<Status> comments_comp;
        private ILikeableComponent<Status> likes_comp;
        private ICommentServiceProvider comments;
        private ILikesServiceProvider likes;

        public ICommentServiceProvider Comments => this.comments;
        public ILikesServiceProvider Likes => this.likes;

        public StatusService(
            MongoDBContext db,
            ICommentServiceProvider comment_provider,
            ILikesServiceProvider likes_provider) : base(db, ModelCollectionName.Status) {
            this.comments_comp = new CommentableServiceComponent<Status>(this, comment_provider);
            this.likes_comp = new LikeableServiceComponent<Status>(this, likes_provider);
            this.comments = comment_provider;
            this.likes = likes_provider;
        }

        #region protected methods

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

        protected UpdateDefinition<Status> DefineImagesUpdate(BaseImage image, bool add = true) {
            var def = Builders<Status>.Update.Set(i => i.UpdateTime, DateTime.UtcNow.ToUnix());
            return add ?
                def.Push(i => i.Images, image) :
                def.Pull(i => i.Images, image);
        }

        #endregion

        public async ValueTask<(IEnumerable<Status> entry, JsonStatus status, Error error)> GetCollectionAbstractAsync(long? before, int? take) {
            var (coll, succeed, error) = await this.GetPublishedCollectionsAsync(before, take.GetValueOrDefault());
            if (!succeed)
                return (default(IList<Status>), JsonStatus.StatusCollectionLoadFailed, error);
            return (coll.Select(i => {
                i.Images = i.Images.Select(m => new BaseImage { Thumbnail = m.Thumbnail, IsFile = m.IsFile }).ToList();
                return i;
            }), JsonStatus.Succeed, Error.Empty);
        }

        public (IEnumerable<Status> entry, JsonStatus status, Error error) GetCollectionAbstract(Expression<Func<Status, bool>> where = null) {
            var (coll, succeed, error) = this.GetPublishedCollections(where);
            if (!succeed)
                return (default(IList<Status>), JsonStatus.StatusCollectionLoadFailed, error);
            return (coll.Select(i => {
                i.Images = i.Images.Select(m => new BaseImage { Thumbnail = m.Thumbnail, IsFile = m.IsFile }).ToList();
                return i;
            }), JsonStatus.Succeed, Error.Empty);
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
            var (coll, succ02, _) = this.comments.GetCollections(i => i.ParentID == entry.ID && i.ParentType == ModelType.Status && i.State == EntryState.Published);
            if (succ02)
                entry.Comments = coll.OrderBy(i => i.CreateTime).ToList();
            var (likers, succ03, _) = this.likes.GetCollections(i => i.ParentID == entry.ID && i.TargetType == ModelType.Status);
            if (succ03)
                entry.Likes = likers;
            return (entry, JsonStatus.Succeed, Error.Empty);
        }

        public async ValueTask<(JsonStatus status, Error error)> DeleteStatusAsync(string id) {
            var (entry, succeed, error) = await this.FindOneDeleteAsync(id);
            if (!succeed)
                return (JsonStatus.DeleteEntryFailed, error);
            await this.comments.DeleteAllAsync(i => i.ParentID == id);
            await this.likes.DeleteAllAsync(i => i.ParentID == id);
            return (JsonStatus.Succeed, Error.Empty);
        }

        public async ValueTask<(JsonStatus status, Error error)> AddCommentAsync(string id, Comment comment) {
            return await this.comments_comp.AddCommentAsync(id, comment);
        }

        public async ValueTask<(JsonStatus status, Error error)> RemoveCommentAsync(string id, string uid, string comment_id) {
            return await this.comments_comp.RemoveCommentAsync(id, uid, comment_id);
        }

        public async ValueTask<(string user_id, JsonStatus status, Error error)> LikeAsync(string id, LikesEntry like) {
            return await this.likes_comp.LikeAsync(id, like);
        }
    }

}
