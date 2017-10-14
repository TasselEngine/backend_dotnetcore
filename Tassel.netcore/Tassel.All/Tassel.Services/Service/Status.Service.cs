using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Model.Utils;
using Tassel.Services.Contract;
using Tassel.Services.Contract.Providers;

namespace Tassel.Services.Service {

    public class StatusService : BaseService<Status>, IStatusService {

        private ICommentServiceProvider comments;
        private ILikesServiceProvider likes;

        public ICommentServiceProvider Comments => this.comments;

        public ILikesServiceProvider Likes => this.likes;

        public StatusService(MongoDBContext db, ICommentServiceProvider coms, ILikesServiceProvider likes) :base(db, ModelCollectionName.Status) {
            this.comments = coms;
            this.likes = likes;
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

    }

}
