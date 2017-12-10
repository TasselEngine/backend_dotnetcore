using BWS.Utils.NetCore.Expressions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Model.Utils;
using Tassel.Services.Contract;

namespace Tassel.Services.Service {

    /// <summary>
    /// Support Tag-Delete feature for page entry base.
    /// </summary>
    /// <typeparam name="T">type extends by DeleteSafelyBase</typeparam>
    public class LogicallyDeleteBase<T> : BaseService<T>, ILogicalDeleteService<T, Error> where T : AccessControllableBase {

        public LogicallyDeleteBase(MongoDBContext db, string collName) : base(db, collName) { }

        /// <summary>
        /// Delete entry safely (tag-delete) by id.
        /// </summary>
        /// <param name="entry_id">key</param>
        /// <returns></returns>
        public (bool succeed, Error error) SafeDeleteOneByID(string entry_id) {
            try {
                var result = this.collection.FindOneAndUpdate(i => i.ID == entry_id, Builders<T>.Update.Set(i => i.State, EntryState.Deleted));
                if (result == null)
                    return (false, Error.Create(Errors.DeleteEntryFailed, Errors.EntryNotExist));
                if (result.State != EntryState.Deleted)
                    return (false, Error.Create(Errors.DeleteEntryFailed));
                else
                    return (true, Error.Empty);
            } catch (Exception e) {
                return (false, Error.Create(Errors.DeleteEntryFailed, e.Message));
            }
        }

        /// <summary>
        /// Delete entry safely (tag-delete) by id [ Async Version ].
        /// </summary>
        /// <param name="entry_id">key</param>
        /// <returns></returns>
        public async ValueTask<(bool succeed, Error error)> SafeDeleteOneByIDAsync(string entry_id) {
            try {
                var result = await this.collection.FindOneAndUpdateAsync(i => i.ID == entry_id, Builders<T>.Update.Set(i => i.State, EntryState.Deleted));
                if (result == null)
                    return (false, Error.Create(Errors.DeleteEntryFailed, Errors.EntryNotExist));
                if (result.State != EntryState.Deleted)
                    return (false, Error.Create(Errors.DeleteEntryFailed));
                else
                    return (true, Error.Empty);
            } catch (Exception e) {
                return (false, Error.Create(Errors.DeleteEntryFailed, e.Message));
            }
        }

        /// <summary>
        /// Delete entry safely (tag-delete) by where filter.
        /// </summary>
        /// <param name="filters">where filter</param>
        /// <returns></returns>
        public (bool succeed, Error error) SafeDeleteOneByFilter(Expression<Func<T, bool>> filters) {
            try {
                var result = this.collection.FindOneAndUpdate(filters, Builders<T>.Update.Set(i => i.State, EntryState.Deleted));
                if (result == null)
                    return (false, Error.Create(Errors.DeleteEntryFailed, Errors.EntryNotExist));
                if (result.State != EntryState.Deleted)
                    return (false, Error.Create(Errors.DeleteEntryFailed));
                else
                    return (true, Error.Empty);
            } catch (Exception e) {
                return (false, Error.Create(Errors.DeleteEntryFailed, e.Message));
            }
        }

        /// <summary>
        /// Delete entry safely (tag-delete) by where filter [ Async Version ].
        /// </summary>
        /// <param name="filters">where filter</param>
        /// <returns></returns>
        public async ValueTask<(bool succeed, Error error)> SafeDeleteOneByFilterAsync(Expression<Func<T, bool>> filters) {
            try {
                var result = await this.collection.FindOneAndUpdateAsync(filters, Builders<T>.Update.Set(i => i.State, EntryState.Deleted));
                if (result == null)
                    return (false, Error.Create(Errors.DeleteEntryFailed, Errors.EntryNotExist));
                if (result.State != EntryState.Deleted)
                    return (false, Error.Create(Errors.DeleteEntryFailed));
                else
                    return (true, Error.Empty);
            } catch (Exception e) {
                return (false, Error.Create(Errors.DeleteEntryFailed, e.Message));
            }
        }

        /// <summary>
        /// Get the collections are published (in tag-delete mode entries coll) where the filter is passed with skip and take params.
        /// </summary>
        /// <param name="where">filter</param>
        /// <param name="skip">number to skip</param>
        /// <param name="take">nnumber to take</param>
        /// <returns></returns>
        public (IList<T> collection, bool succeed, Error error) GetPublishedCollections(Expression<Func<T, bool>> where = null, int? skip = null, int? take = null) {
            return this.GetCollections((where ?? (w => true)).And(w => w.State == EntryState.Published), skip, take);
        }

        /// <summary>
        /// Get the collections are published (in tag-delete mode entries coll) where the filter is passed with skip and take params [ Async Version ].
        /// </summary>
        /// <param name="where">filter</param>
        /// <param name="skip">number to skip</param>
        /// <param name="take">nnumber to take</param>
        /// <returns></returns>
        public async ValueTask<(IList<T> collection, bool succeed, Error error)> GetPublishedCollectionsAsync(Expression<Func<T, bool>> where = null, int? skip = null, int? take = null) {
            return await this.GetCollectionsAsync((where ?? (m => true)).And(m => m.State == EntryState.Published), skip, take);
        }

        /// <summary>
        /// Get the collections are published (in tag-delete mode entries coll) where the filter is passed with stamp( less than stamp) take params [ Async Version ].
        /// </summary>
        /// <param name="where">filter</param>
        /// <param name="skip">number to skip</param>
        /// <param name="take">nnumber to take</param>
        /// <returns></returns>
        public async ValueTask<(IList<T> collection, bool succeed, Error error)> GetPublishedCollectionsAsync(long stamp, int? take = null) {
            Expression<Func<T, bool>> where = i => i.CreateTime < stamp;
            if (stamp <= 0)
                where = i => true;
            return await this.GetCollectionsAsync(where.And(m => m.State == EntryState.Published), null, take);
        }

    }

}
