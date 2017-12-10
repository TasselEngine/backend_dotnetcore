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

    public class LikeableService<T> : LogicallyDeleteBase<T>,  ILikeableService<T, Error, LikesEntry> where T : BaseLikesModel  {

        public ILikesServiceProvider Likes => this.likes;

        private ILikesServiceProvider likes;

        public LikeableService(MongoDBContext db, ILikesServiceProvider likes_prd, string collName) : base(db, collName) {
            this.likes = likes_prd;
        }

        protected UpdateDefinition<T> DefineLikersUpdate(string user_uuid, bool add = true) {
            var def = Builders<T>.Update;
            return add ?
                def.AddToSet(i => i.LikerIDs, user_uuid) :
                def.Pull(i => i.LikerIDs, user_uuid);
        }

        public async ValueTask<(string user_id, JsonStatus status, Error error)> LikeAsync(string id, LikesEntry like) {
            var (entry, succeed, error) = await this.FindOneByIDAsync(id);
            if (!succeed)
                return (default(string), JsonStatus.StatusNotFound, error);
            if (entry.LikerIDs.Contains(like.User.UUID) || entry.Likes.FirstOrDefault(i => i.User.UUID == like.User.UUID) != null) {
                (succeed, error) = await this.likes.DeleteAllByIDsAsync(id, like.User.UUID);
                if (!succeed)
                    return (default(string), JsonStatus.LikesRemoveFailed, error);
                (_, succeed, error) = await this.UpdateOneAsync(id, null, this.DefineLikersUpdate(like.User.UUID, false));
                if (!succeed)
                    return (default(string), JsonStatus.LikesRemoveFailed, error);
                return ("deleted", JsonStatus.Succeed, Error.Empty);
            } else {
                (_, succeed, error) = await this.likes.InsertOneAsync(like);
                if (!succeed)
                    return (default(string), JsonStatus.LikesAddFailed, error);
                (_, succeed, error) = await this.UpdateOneAsync(id, null, this.DefineLikersUpdate(like.User.UUID));
                if (!succeed)
                    return (default(string), JsonStatus.LikesAddFailed, error);
                return (like.User.UUID, JsonStatus.Succeed, Error.Empty);
            }
        }

    }

}
