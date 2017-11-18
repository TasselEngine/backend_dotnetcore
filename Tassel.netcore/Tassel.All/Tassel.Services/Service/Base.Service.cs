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

    /// <summary>
    /// The ABC to support the definition creators feature for Mongo DB.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BsonCRUDBase<T> where T : BaseModel {

        /// <summary>
        /// Provide a generic update feature.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        protected virtual UpdateDefinition<T> DefineUpdate(T entry) {
            return Builders<T>.Update.Set(i => i.UpdateTime, DateTime.UtcNow.ToUnix());
        }

    }

    /// <summary>
    /// The ABC to provide generic features for services.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseService<T> : BsonCRUDBase<T>, IBusinessService<T, Error> where T : BaseModel {

        protected IMongoDatabase mdb;
        protected IMongoCollection<T> collection;

        public BaseService(MongoDBContext db, string collName) {
            this.mdb = db.DB;
            this.collection = mdb.GetCollection<T>(collName);
        }

        /// <summary>
        /// Find entry by _id.
        /// </summary>
        /// <param name="id">key of entry</param>
        /// <returns></returns>
        public (T entry, bool succeed, Error error) FindOneByID(string id) {
            try {
                var entry = this.collection.Find(i => i.ID == id).FirstOrDefault();
                return (entry, true, Error.Empty);
            } catch (Exception e) {
                return (default(T), false, Error.Create(Errors.QueryEntryFailed, e.Message));
            }
        }

        /// <summary>
        /// Find entry by _id [ Async Version ].
        /// </summary>
        /// <param name="id">key of entry</param>
        /// <returns></returns>
        public async ValueTask<(T entry, bool succeed, Error error)> FindOneByIDAsync(string id) {
            try {
                var entry = (await this.collection.FindAsync(i => i.ID == id)).FirstOrDefault();
                return (entry, true, Error.Empty);
            } catch (Exception e) {
                return (default(T), false, Error.Create(Errors.QueryEntryFailed, e.Message));
            }
        }

        /// <summary>
        /// Get the collections where the filter is passed with skip and take params.
        /// </summary>
        /// <param name="where">filter</param>
        /// <param name="skip">number to skip</param>
        /// <param name="take">nnumber to take</param>
        /// <returns></returns>
        public (IList<T> collection, bool succeed, Error error) GetCollections(
            Expression<Func<T, bool>> where = null,
            int? skip = null,
            int? take = null) {

            where = where ?? (i => true);
            try {
                var coll = this.collection.AsQueryable().OrderByDescending(i => i.CreateTime).Where(where);
                if (skip != null)
                    coll = coll.Skip(skip.GetValueOrDefault());
                if (take != null)
                    coll = coll.Take(take.GetValueOrDefault());
                return (coll.ToList(), true, Error.Empty);
            } catch (Exception e) {
                return (default(IList<T>), false, Error.Create(Errors.GetEntryCollFailed, e.Message));
            }
        }

        /// <summary>
        /// Get the collections where the filter is passed with skip and take params [ Async Version ].
        /// </summary>
        /// <param name="where">filter</param>
        /// <param name="skip">number to skip</param>
        /// <param name="take">nnumber to take</param>
        /// <returns></returns>
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

        /// <summary>
        /// Get the collections where the filter and stamp is passed take params.
        /// </summary>
        /// <param name="stamp"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public (IList<T> collection, bool succeed, Error error) GetCollections(long stamp, int? take = null) {
            try {
                var coll = this.collection.AsQueryable().OrderByDescending(i => i.CreateTime).Where(i=>i.CreateTime<stamp);
                if (take != null)
                    coll = coll.Take(take.GetValueOrDefault());
                return (coll.ToList(), true, Error.Empty);
            } catch (Exception e) {
                return (default(IList<T>), false, Error.Create(Errors.GetEntryCollFailed, e.Message));
            }
        }

        /// <summary>
        /// Get the collections where the filter and stamp is passed take params. [ Async Version ].
        /// </summary>
        /// <param name="stamp"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public async ValueTask<(IList<T> collection, bool succeed, Error error)> GetCollectionsAsync(long stamp, int? take = null) {
            try {
                using (var coll_async = this.collection.AsQueryable().ToCursorAsync()) {
                    IEnumerable<T> coll = (await coll_async).ToEnumerable().Where(i=>i.CreateTime<stamp).OrderByDescending(i => i.CreateTime);
                    if (take != null)
                        coll = coll.Take(take.GetValueOrDefault());
                    return (coll.ToList(), true, Error.Empty);
                }
            } catch (Exception e) {
                return (default(IList<T>), false, Error.Create(Errors.GetEntryCollFailed, e.Message));
            }
        }

        /// <summary>
        /// Insert item.
        /// </summary>
        /// <param name="entry">the entry to be insert.</param>
        /// <returns></returns>
        public (T entry, bool succeed, Error error) InsertOne(T entry) {
            try {
                this.collection.InsertOne(entry);
                return (entry, true, Error.Empty);
            } catch (Exception e) {
                return (default(T), false, Error.Create(Errors.InsertOneFailed, e.Message));
            }
        }

        /// <summary>
        /// Insert item [ Async Version ].
        /// </summary>
        /// <param name="entry">the entry to be insert.</param>
        /// <returns></returns>
        public async ValueTask<(T entry, bool succeed, Error error)> InsertOneAsync(T entry) {
            try {
                await this.collection.InsertOneAsync(entry);
                return (entry, true, Error.Empty);
            } catch (Exception e) {
                return (default(T), false, Error.Create(Errors.InsertOneFailed, e.Message));
            }
        }

        /// <summary>
        /// Update an entry with definition(if def is null, the override definition will be used) , action model(toDo) and entry id.
        /// </summary>
        /// <param name="id">entry id of target to be update</param>
        /// <param name="toDo">model contains the new changes</param>
        /// <param name="updateDef">provider to update</param>
        /// <returns></returns>
        public (string entry_id, bool succeed, Error error) UpdateOne(string id, T toDo = null, UpdateDefinition<T> updateDef = null) {
            try {
                var result = this.collection.UpdateOne(i => i.ID == id, updateDef ?? this.DefineUpdate(toDo));
                if (result.IsAcknowledged)
                    return (result?.UpsertedId?.AsString, true, Error.Empty);
                return (default(string), false, Error.Create(Errors.UpdateEntryFailed));
            } catch (Exception e) {
                return (default(string), false, Error.Create(Errors.UpdateEntryFailed, e.Message));
            }
        }

        /// <summary>
        /// Update an entry with definition(if def is null, the override definition will be used) , action model(toDo) and entry id [ Async Version ].
        /// </summary>
        /// <param name="id">entry id of target to be update</param>
        /// <param name="toDo">model contains the new changes</param>
        /// <param name="updateDef">provider to update</param>
        /// <returns></returns>
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

        /// <summary>
        /// Update an entry with definition(if def is null, the override definition will be used) , action model(toDo) and entry id. Then you can get the POCO entry.
        /// </summary>
        /// <param name="id">entry id of target to be update</param>
        /// <param name="toDo">model contains the new changes</param>
        /// <param name="updateDef">provider to update</param>
        /// <returns></returns>
        public (T outEntry, bool succeed, Error error) FindOneUpdate(string id, T toDo = null, UpdateDefinition<T> updateDef = null) {
            try {
                var result = this.collection.FindOneAndUpdate(i => i.ID == id, updateDef ?? this.DefineUpdate(toDo));
                if (result == null)
                    return (default(T), false, Error.Create(Errors.UpdateEntryFailed, Errors.FindUpdateFailed));
                return (result, true, Error.Empty);
            } catch (Exception e) {
                return (default(T), false, Error.Create(Errors.UpdateEntryFailed, e.Message));
            }
        }

        /// <summary>
        /// Update an entry with definition(if def is null, the override definition will be used) , action model(toDo) and entry id. Then you can get the POCO entry [ Async Version ].
        /// </summary>
        /// <param name="id">entry id of target to be update</param>
        /// <param name="toDo">model contains the new changes</param>
        /// <param name="updateDef">provider to update</param>
        /// <returns></returns>
        public async ValueTask<(T outEntry, bool succeed, Error error)> FindOneUpdateAsync(string id, T toDo = null, UpdateDefinition<T> updateDef = null) {
            try {
                var result = await this.collection.FindOneAndUpdateAsync(i => i.ID == id, updateDef ?? this.DefineUpdate(toDo));
                if (result == null)
                    return (default(T), false, Error.Create(Errors.UpdateEntryFailed, Errors.FindUpdateFailed));
                return (result, true, Error.Empty);
            } catch (Exception e) {
                return (default(T), false, Error.Create(Errors.UpdateEntryFailed, e.Message));
            }
        }

        /// <summary>
        /// Delete entry by id.
        /// </summary>
        /// <param name="id">key</param>
        /// <returns></returns>
        public (bool succeed, Error error) DeleteOne(string id) {
            try {
                var result = this.collection.DeleteOne(i => i.ID == id);
                if (result.IsAcknowledged)
                    if (result.DeletedCount == 0)
                        return (false, Error.Create(Errors.DeleteEntryFailed, Errors.EntryFailedIsDeleted));
                    else
                        return (true, Error.Empty);
                return (false, Error.Create(Errors.DeleteEntryFailed));
            } catch (Exception e) {
                return (false, Error.Create(Errors.DeleteEntryFailed, e.Message));
            }
        }

        /// <summary>
        /// Delete entry by id [ Async Version ].
        /// </summary>
        /// <param name="id">key</param>
        /// <returns></returns>
        public async ValueTask<(bool succeed, Error error)> DeleteOneAsync(string id) {
            try {
                var result = await this.collection.DeleteOneAsync(i => i.ID == id);
                if (result.IsAcknowledged)
                    if (result.DeletedCount == 0)
                        return (false, Error.Create(Errors.DeleteEntryFailed, Errors.EntryFailedIsDeleted));
                    else
                        return (true, Error.Empty);
                return (false, Error.Create(Errors.DeleteEntryFailed));
            } catch (Exception e) {
                return (false, Error.Create(Errors.DeleteEntryFailed, e.Message));
            }
        }

        /// <summary>
        /// Delete entry by where filter(if not provide , i=>false will be used).
        /// </summary>
        /// <param name="filters">the filter to find the entry</param>
        /// <returns></returns>
        public (bool succeed, Error error) DeleteOne(Expression<Func<T, bool>> filters) {
            try {
                var result = this.collection.DeleteOne(Builders<T>.Filter.Where(filters));
                if (result.IsAcknowledged)
                    if (result.DeletedCount == 0)
                        return (false, Error.Create(Errors.DeleteEntryFailed, Errors.EntryFailedIsDeleted));
                    else
                        return (true, Error.Empty);
                return (false, Error.Create(Errors.DeleteEntryFailed));
            } catch (Exception e) {
                return (false, Error.Create(Errors.DeleteEntryFailed, e.Message));
            }
        }

        /// <summary>
        /// Delete entry by where filter(if not provide , i=>false will be used) [ Async Version ].
        /// </summary>
        /// <param name="filters">the filter to find the entry</param>
        /// <returns></returns>
        public async ValueTask<(bool succeed, Error error)> DeleteOneAsync(Expression<Func<T, bool>> filters) {
            try {
                var result = await this.collection.DeleteOneAsync(Builders<T>.Filter.Where(filters));
                if (result.IsAcknowledged)
                    if (result.DeletedCount == 0)
                        return (false, Error.Create(Errors.DeleteEntryFailed, Errors.EntryFailedIsDeleted));
                    else
                        return (true, Error.Empty);
                return (false, Error.Create(Errors.DeleteEntryFailed));
            } catch (Exception e) {
                return (false, Error.Create(Errors.DeleteEntryFailed, e.Message));
            }
        }

        /// <summary>
        /// Delete an entry after find it.
        /// </summary>
        /// <param name="id">entry id of target to be deleted</param>
        /// <returns></returns>
        public (T outEntry, bool succeed, Error error) FindOneDelete(string id) {
            try {
                var result = this.collection.FindOneAndDelete(i => i.ID == id);
                if (result == null)
                    return (default(T), false, Error.Create(Errors.DeleteEntryFailed, Errors.FindDeleteFailed));
                return (result, true, Error.Empty);
            } catch (Exception e) {
                return (default(T), false, Error.Create(Errors.DeleteEntryFailed, e.Message));
            }
        }

        /// <summary>
        ///  Delete an entry after find it [ Async Version ].
        /// </summary>
        /// <param name="id">entry id of target to be deleted</param>
        /// <returns></returns>
        public async ValueTask<(T outEntry, bool succeed, Error error)> FindOneDeleteAsync(string id) {
            try {
                var result = await this.collection.FindOneAndDeleteAsync(i => i.ID == id);
                if (result == null)
                    return (default(T), false, Error.Create(Errors.DeleteEntryFailed, Errors.FindDeleteFailed));
                return (result, true, Error.Empty);
            } catch (Exception e) {
                return (default(T), false, Error.Create(Errors.DeleteEntryFailed, e.Message));
            }
        }

        /// <summary>
        /// Delete an entry after find it.
        /// </summary>
        /// <param name="filter">the filter to find the entry</param>
        /// <returns></returns>
        public (T outEntry, bool succeed, Error error) FindOneDelete(Expression<Func<T, bool>> filter) {
            try {
                var result = this.collection.FindOneAndDelete(filter);
                if (result == null)
                    return (default(T), false, Error.Create(Errors.DeleteEntryFailed, Errors.FindDeleteFailed));
                return (result, true, Error.Empty);
            } catch (Exception e) {
                return (default(T), false, Error.Create(Errors.DeleteEntryFailed, e.Message));
            }
        }

        /// <summary>
        ///  Delete an entry after find it [ Async Version ].
        /// </summary>
        /// <param name="filter">the filter to find the entry</param>
        /// <returns></returns>
        public async ValueTask<(T outEntry, bool succeed, Error error)> FindOneDeleteAsync(Expression<Func<T, bool>> filter) {
            try {
                var result = await this.collection.FindOneAndDeleteAsync(filter);
                if (result == null)
                    return (default(T), false, Error.Create(Errors.DeleteEntryFailed, Errors.FindDeleteFailed));
                return (result, true, Error.Empty);
            } catch (Exception e) {
                return (default(T), false, Error.Create(Errors.DeleteEntryFailed, e.Message));
            }
        }

        /// <summary>
        /// Delete entry by where filter .
        /// </summary>
        /// <param name="filters">the filter to find the entry</param>
        /// <returns></returns>
        public (bool succeed, Error error) DeleteAll(Expression<Func<T, bool>> filters) {
            try {
                var result = this.collection.DeleteMany(Builders<T>.Filter.Where(filters));
                if (result.IsAcknowledged)
                    if (result.DeletedCount == 0)
                        return (false, Error.Create(Errors.DeleteEntryFailed, Errors.EntryFailedIsDeleted));
                    else
                        return (true, Error.Empty);
                return (false, Error.Create(Errors.DeleteEntryFailed));
            } catch (Exception e) {
                return (false, Error.Create(Errors.DeleteEntryFailed, e.Message));
            }
        }

        /// <summary>
        /// Delete entry by where filter [ Async Version ].
        /// </summary>
        /// <param name="filters">the filter to find the entry</param>
        /// <returns></returns>
        public async ValueTask<(bool succeed, Error error)> DeleteAllAsync(Expression<Func<T, bool>> filters) {
            try {
                var result = await this.collection.DeleteManyAsync(Builders<T>.Filter.Where(filters));
                if (result.IsAcknowledged)
                    if (result.DeletedCount == 0)
                        return (false, Error.Create(Errors.DeleteEntryFailed, Errors.EntryFailedIsDeleted));
                    else
                        return (true, Error.Empty);
                return (false, Error.Create(Errors.DeleteEntryFailed));
            } catch (Exception e) {
                return (false, Error.Create(Errors.DeleteEntryFailed, e.Message));
            }
        }

    }

}
