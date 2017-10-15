using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tassel.Model.Utils;
using Tassel.Services.Contract;
using System.Linq.Expressions;
using System.Linq;
using System.Reactive.Linq;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using BWS.Utils.NetCore.Format;

namespace Tassel.Services.Service {

    public abstract class BsonCRUDBase<T> where T : BaseModel {

        protected virtual UpdateDefinition<T> DefineUpdate(T entry) {
            return Builders<T>.Update.Set(i => i.UpdateTime, DateTime.UtcNow.ToUnix());
        }

    }

    public abstract class BaseService<T> : BsonCRUDBase<T>, IBusinessService<T, Error> where T : BaseModel {

        protected IMongoDatabase mdb;
        protected IMongoCollection<T> collection;

        public BaseService(MongoDBContext db, string collName) {
            this.mdb = db.DB;
            this.collection = mdb.GetCollection<T>(collName);
        }

        public (T entry, bool succeed, Error error) FindOneByID(string id) {
            try {
                var entry = this.collection.Find(i => i.ID == id).FirstOrDefault();
                return (entry, true, Error.Empty);
            } catch (Exception e) {
                return (default(T), false, Error.Create(Errors.QueryEntryFailed, e.Message));
            }
        }

        public async ValueTask<(T entry, bool succeed, Error error)> FindOneByIDAsync(string id) {
            try {
                var entry = (await this.collection.FindAsync(i => i.ID == id)).FirstOrDefault();
                return (entry, true, Error.Empty);
            } catch (Exception e) {
                return (default(T), false, Error.Create(Errors.QueryEntryFailed, e.Message));
            }
        }

        public (IList<T> collection, bool succeed, Error error) GetCollections(
            Expression<Func<T, bool>> where = null, 
            int? skip = null, 
            int? take = null) {

            where = where ?? (i => true);
            try {
                var coll = this.collection.AsQueryable().OrderByDescending(i=>i.CreateTime).Where(where);
                if (skip != null)
                    coll = coll.Skip(skip.GetValueOrDefault());
                if (take != null)
                    coll = coll.Take(take.GetValueOrDefault());
                return (coll.ToList(), true, Error.Empty);
            } catch (Exception e) {
                return (default(IList<T>), false, Error.Create(Errors.GetEntryCollFailed, e.Message));
            }
        }

        public async ValueTask<(IList<T> collection, bool succeed, Error error)> GetCollectionsAsync(
            Expression<Func<T, bool>> where = null, 
            int? skip = null,
            int? take = null) {

            where = where ?? (i => true);
            try {
                using (var coll_async = this.collection.AsQueryable().ToCursorAsync()) {
                    IEnumerable<T> coll = (await coll_async).ToEnumerable().OrderByDescending(i => i.CreateTime).Where(where.Compile());
                    if (skip != null)
                        coll = coll.Skip(skip.GetValueOrDefault());
                    if (take != null)
                        coll = coll.Take(take.GetValueOrDefault());
                    return (coll.ToList(), true, Error.Empty);
                }
            } catch (Exception e) {
                return (default(IList<T>), false, Error.Create(Errors.GetEntryCollFailed, e.Message));
            }
        }

        public (T entry, bool succeed, Error error) InsertOne(T entry) {
            try {
                this.collection.InsertOne(entry);
                return (entry, true, Error.Empty);
            } catch (Exception e) {
                return (default(T), false, Error.Create(Errors.InsertOneFailed, e.Message));
            }
        }

        public async ValueTask<(T entry, bool succeed, Error error)> InsertOneAsync(T entry) {
            try {
                await this.collection.InsertOneAsync(entry);
                return (entry, true, Error.Empty);
            } catch (Exception e) {
                return (default(T), false, Error.Create(Errors.InsertOneFailed, e.Message));
            }
        }

        public (string entry_id, bool succeed, Error error) UpdateOne(string id, T toDo = null, UpdateDefinition<T> updateDef = null) {
            try {
                var result = this.collection.UpdateOne(i=>i.ID == id, updateDef ?? this.DefineUpdate(toDo));
                if(result.IsAcknowledged)
                    return (result?.UpsertedId?.AsString, true, Error.Empty);
                return (default(string), false, Error.Create(Errors.UpdateEntryFailed));
            } catch (Exception e) {
                return (default(string), false, Error.Create(Errors.UpdateEntryFailed, e.Message));
            }
        }

        public async ValueTask<(string entry_id, bool succeed, Error error)> UpdateOneAsync(string id, T toDo = null, UpdateDefinition<T> updateDef = null) {
            try {
                var result = await this.collection.UpdateOneAsync(i => i.ID == id, updateDef ?? this.DefineUpdate(toDo));
                if (result.IsAcknowledged)
                    return (result?.UpsertedId?.AsString, true, Error.Empty);
                return (default(string), false, Error.Create(Errors.UpdateEntryFailed));
            } catch (Exception e) {
                return (default(string), false, Error.Create(Errors.UpdateEntryFailed, e.Message));
            }
        }

        public (T outEntry, bool succeed, Error error) FindOneUpdate(string id, T toDo = null, UpdateDefinition<T> updateDef = null) {
            try {
                var result = this.collection.FindOneAndUpdate(i => i.ID == id, updateDef ?? this.DefineUpdate(toDo));
                return (result, true, Error.Empty);
            } catch (Exception e) {
                return (default(T), false, Error.Create(Errors.UpdateEntryFailed, e.Message));
            }
        }

        public async ValueTask<(T outEntry, bool succeed, Error error)> FindOneUpdateAsync(string id, T toDo = null, UpdateDefinition<T> updateDef = null) {
            try {
                var result = await this.collection.FindOneAndUpdateAsync(i => i.ID == id, updateDef ?? this.DefineUpdate(toDo));
                return (result, true, Error.Empty);
            } catch (Exception e) {
                return (default(T), false, Error.Create(Errors.UpdateEntryFailed, e.Message));
            }
        }

        public (bool succeed, Error error) DeleteOne(string entry_id) {
            try {
                var result = this.collection.DeleteOne(i => i.ID == entry_id);
                if(result.IsAcknowledged)
                    return (true, Error.Empty);
                return (false, Error.Create(Errors.DeleteEntryFailed));
            } catch (Exception e) {
                return (false, Error.Create(Errors.DeleteEntryFailed, e.Message));
            }
        }

        public async ValueTask<(bool succeed, Error error)> DeleteOneAsync(string entry_id) {
            try {
                var result = await this.collection.DeleteOneAsync(i => i.ID == entry_id);
                if (result.IsAcknowledged)
                    return (true, Error.Empty);
                return (false, Error.Create(Errors.DeleteEntryFailed));
            } catch (Exception e) {
                return (false, Error.Create(Errors.DeleteEntryFailed, e.Message));
            }
        }
    }

}
