using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Model.Utils;
using Tassel.Services.Contract.Providers;
using Tassel.Services.Service;

namespace Tassel.Services.Providers {

    public class LikesService : BaseService<LikesEntry>, ILikesServiceProvider {

        public LikesService(MongoDBContext db) :base(db, ModelCollectionName.Likes) { }

        public (bool succeed, Error error) DeleteAllByIDs(string target_id, string user_id) {
            try {
                var result = this.collection.DeleteMany(Builders<LikesEntry>.Filter.Where(i => i.ParentID == target_id && i.User.UUID == user_id));
                if (result.IsAcknowledged)
                    return (true, Error.Empty);
                return (false, Error.Create(Errors.DeleteEntryFailed));
            } catch(Exception e) {
                return (false, Error.Create(Errors.DeleteEntryFailed, e.Message));
            }
        }

        public async ValueTask<(bool succeed, Error error)> DeleteAllByIDsAsync(string target_id, string user_id) {
            try {
                var result = await this.collection.DeleteManyAsync(Builders<LikesEntry>.Filter.Where(i => i.ParentID == target_id && i.User.UUID == user_id));
                if (result.IsAcknowledged)
                    return (true, Error.Empty);
                return (false, Error.Create(Errors.DeleteEntryFailed));
            } catch (Exception e) {
                return (false, Error.Create(Errors.DeleteEntryFailed, e.Message));
            }
        }
    }

}
