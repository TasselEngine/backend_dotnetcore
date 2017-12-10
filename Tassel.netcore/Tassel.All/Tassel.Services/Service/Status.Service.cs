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
using Tassel.Services.Contract;
using Tassel.Services.Contract.Providers;

namespace Tassel.Services.Service {

    public class StatusService : CommentableAndLikeService<Status>, IStatusService {

        private ICommentServiceProvider comments;
        private ILikesServiceProvider likes;

        public StatusService(
            MongoDBContext db,
            ICommentServiceProvider coms,
            ILikesServiceProvider likes) : base(db, coms, likes, ModelCollectionName.Status) {
            this.comments = coms;
            this.likes = likes;
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

        public async ValueTask<(IEnumerable<Status> entry, JsonStatus status, Error error)> GetCollectionAbstractAsync(long stamp, int? take) {
            var (coll, succeed, error) = await this.GetPublishedCollectionsAsync(stamp, take.GetValueOrDefault());
            if (!succeed)
                return (default(IList<Status>), JsonStatus.StatusCollectionLoadFailed, error);
            return (coll.Select(i => {
                i.Images = i.Images.Select(m => new BaseImage { Thumbnail = m.Thumbnail, IsFile = m.IsFile }).ToList();
                return i;
            }), JsonStatus.Succeed, Error.Empty);
        }

        public async ValueTask<(IEnumerable<Status> entry, JsonStatus status, Error error)> GetCollectionAbstractAsync(Expression<Func<Status, bool>> where = null) {
            var (coll, succeed, error) = await this.GetPublishedCollectionsAsync(where);
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
            var (coll, succ02, _) = await this.comments.GetCollectionsAsync(
                i => i.ParentID == entry.ID && i.ParentType == ModelType.Status && i.State == EntryState.Published);
            if (succ02) 
                entry.Comments = coll.OrderBy(i => i.CreateTime).ToList();
            var (likers, succ03, _) = await this.likes.GetCollectionsAsync(
                i => i.ParentID == entry.ID && i.TargetType == ModelType.Status);
            if (succ03)
                entry.Likes = likers;
            return (entry, JsonStatus.Succeed, Error.Empty);
        }

        public async ValueTask<(JsonStatus status, Error error)> DeleteStatusAsync(string id) {
            var (entry, succeed, error) = await this.FindOneDeleteAsync(id);
            if(!succeed)
                return (JsonStatus.DeleteEntryFailed, error);
            await this.Comments.DeleteAllAsync(i => i.ParentID == id);
            await this.Likes.DeleteAllAsync(i => i.ParentID == id);
            return (JsonStatus.Succeed, Error.Empty);
        }
    }

}
