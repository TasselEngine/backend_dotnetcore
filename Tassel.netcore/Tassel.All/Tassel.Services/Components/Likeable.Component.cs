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
using Tassel.Services.Contract.Components;
using Tassel.Services.Contract.Providers;

namespace Tassel.Services.Components {

    public interface ILikeableComponent<T> : ILikeableServiceComponent<T, Error, LikesEntry> where T : BaseLikesModel { }

    class LikeableServiceComponent<T> : ILikeableComponent<T> where T : BaseLikesModel {

        private ILogicalDeleteService<T, Error> context;
        private ILikesServiceProvider likes;

        public ILikesServiceProvider Likes => this.likes;

        public LikeableServiceComponent(ILogicalDeleteService<T, Error> context, ILikesServiceProvider likes) {
            this.context = context;
            this.likes = likes;
        }

        protected UpdateDefinition<T> DefineLikersUpdate(string user_uuid, bool add = true) {
            var def = Builders<T>.Update;
            return add ?
                def.AddToSet(i => i.LikerIDs, user_uuid) :
                def.Pull(i => i.LikerIDs, user_uuid);
        }

        public async ValueTask<(string user_id, JsonStatus status, Error error)> LikeAsync(string id, LikesEntry like) {
            var (entry, succeed, error) = await this.context.FindOneByIDAsync(id);
            if (!succeed)
                return (default(string), JsonStatus.EntryNotFound, error);
            if (entry.LikerIDs.Contains(like.User.UUID) || entry.Likes.ToList().FirstOrDefault(i => i.User.UUID == like.User.UUID) != null) {
                (succeed, error) = await this.likes.DeleteAllByIDsAsync(id, like.User.UUID);
                if (!succeed)
                    return (default(string), JsonStatus.LikesRemoveFailed, error);
                (_, succeed, error) = await this.context.UpdateOneAsync(id, null, this.DefineLikersUpdate(like.User.UUID, false));
                if (!succeed)
                    return (default(string), JsonStatus.LikesRemoveFailed, error);
                return ("deleted", JsonStatus.Succeed, Error.Empty);
            } else {
                (_, succeed, error) = await this.likes.InsertOneAsync(like);
                if (!succeed)
                    return (default(string), JsonStatus.LikesAddFailed, error);
                (_, succeed, error) = await this.context.UpdateOneAsync(id, null, this.DefineLikersUpdate(like.User.UUID));
                if (!succeed)
                    return (default(string), JsonStatus.LikesAddFailed, error);
                return (like.User.UUID, JsonStatus.Succeed, Error.Empty);
            }
        }

    }
}
