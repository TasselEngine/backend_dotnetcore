using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Models.BsonModels;

namespace Tassel.Services.Contract {

    /// <summary>
    /// The interface to provide generic features for services.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBusinessService<T, TError> where T : IBaseModel {

        /// <summary>
        /// Insert item.
        /// </summary>
        /// <param name="entry">the entry to be insert.</param>
        /// <returns></returns>
        (T entry, bool succeed, TError error) InsertOne(T entry);

        /// <summary>
        /// Insert item [ Async Version ].
        /// </summary>
        /// <param name="entry">the entry to be insert.</param>
        /// <returns></returns>
        ValueTask<(T entry, bool succeed, TError error)> InsertOneAsync(T entry);

        /// <summary>
        /// Update an entry with definition(if def is null, the override definition will be used) , action model(toDo) and entry id.
        /// </summary>
        /// <param name="id">entry id of target to be update</param>
        /// <param name="toDo">model contains the new changes</param>
        /// <param name="updateDef">provider to update</param>
        /// <returns></returns>
        (string entry_id, bool succeed, TError error) UpdateOne(string id, T toDo = default(T), UpdateDefinition<T> updateDef = null);

        /// <summary>
        /// Update an entry with definition(if def is null, the override definition will be used) , action model(toDo) and entry id [ Async Version ].
        /// </summary>
        /// <param name="id">entry id of target to be update</param>
        /// <param name="toDo">model contains the new changes</param>
        /// <param name="updateDef">provider to update</param>
        /// <returns></returns>
        ValueTask<(string entry_id, bool succeed, TError error)> UpdateOneAsync(string id, T toDo = default(T), UpdateDefinition<T> updateDef = null);

        /// <summary>
        /// Update an entry with definition(if def is null, the override definition will be used) , action model(toDo) and entry id. Then you can get the POCO entry.
        /// </summary>
        /// <param name="id">entry id of target to be update</param>
        /// <param name="toDo">model contains the new changes</param>
        /// <param name="updateDef">provider to update</param>
        /// <returns></returns>
        (T outEntry, bool succeed, TError error) FindOneUpdate(string id, T toDo = default(T), UpdateDefinition<T> updateDef = null);

        /// <summary>
        /// Update an entry with definition(if def is null, the override definition will be used) , action model(toDo) and entry id. Then you can get the POCO entry [ Async Version ].
        /// </summary>
        /// <param name="id">entry id of target to be update</param>
        /// <param name="toDo">model contains the new changes</param>
        /// <param name="updateDef">provider to update</param>
        /// <returns></returns>
        ValueTask<(T outEntry, bool succeed, TError error)> FindOneUpdateAsync(string id, T toDo = default(T), UpdateDefinition<T> updateDef = null);

        /// <summary>
        /// Delete an entry after find it.
        /// </summary>
        /// <param name="id">entry id of target to be deleted</param>
        /// <returns></returns>
        (T outEntry, bool succeed, TError error) FindOneDelete(string id);

        /// <summary>
        ///  Delete an entry after find it [ Async Version ].
        /// </summary>
        /// <param name="id">entry id of target to be deleted</param>
        /// <returns></returns>
        ValueTask<(T outEntry, bool succeed, TError error)> FindOneDeleteAsync(string id);

        /// <summary>
        /// Delete an entry after find it.
        /// </summary>
        /// <param name="filter">the filter to find the entry</param>
        /// <returns></returns>
        (T outEntry, bool succeed, TError error) FindOneDelete(Expression<Func<T, bool>> filter);

        /// <summary>
        ///  Delete an entry after find it [ Async Version ].
        /// </summary>
        /// <param name="filter">the filter to find the entry</param>
        /// <returns></returns>
        ValueTask<(T outEntry, bool succeed, TError error)> FindOneDeleteAsync(Expression<Func<T, bool>> filter);

        /// <summary>
        /// Get the collections where the filter is passed with skip and take params.
        /// </summary>
        /// <param name="where">filter</param>
        /// <param name="skip">number to skip</param>
        /// <param name="take">nnumber to take</param>
        /// <returns></returns>
        (IList<T> collection, bool succeed, TError error) GetCollections(Expression<Func<T, bool>> where = null, int? skip = null, int? take = null);

        /// <summary>
        /// Get the collections where the filter is passed with skip and take params [ Async Version ].
        /// </summary>
        /// <param name="where">filter</param>
        /// <param name="skip">number to skip</param>
        /// <param name="take">nnumber to take</param>
        /// <returns></returns>
        ValueTask<(IList<T> collection, bool succeed, TError error)> GetCollectionsAsync(Expression<Func<T, bool>> where = null, int? skip = null, int? take = null);

        /// <summary>
        /// Get the collections where the filter and stamp is passed take params.
        /// </summary>
        /// <param name="stamp"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        (IList<T> collection, bool succeed, TError error) GetCollections(long stamp, int? take = null);

        /// <summary>
        /// Get the collections where the filter and stamp is passed take params. [ Async Version ].
        /// </summary>
        /// <param name="stamp"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        ValueTask<(IList<T> collection, bool succeed, TError error)> GetCollectionsAsync(long stamp, int? take = null);

        /// <summary>
        /// Find entry by _id.
        /// </summary>
        /// <param name="id">key of entry</param>
        /// <returns></returns>
        (T entry, bool succeed, TError error) FindOneByID(string id);

        /// <summary>
        /// Find entry by _id [ Async Version ].
        /// </summary>
        /// <param name="id">key of entry</param>
        /// <returns></returns>
        ValueTask<(T entry, bool succeed, TError error)> FindOneByIDAsync(string id);

        /// <summary>
        /// Delete entry by id.
        /// </summary>
        /// <param name="entry_id">key</param>
        /// <returns></returns>
        (bool succeed, TError error) DeleteOne(string id);

        /// <summary>
        /// Delete entry by id [ Async Version ].
        /// </summary>
        /// <param name="entry_id">key</param>
        /// <returns></returns>
        ValueTask<(bool succeed, TError error)> DeleteOneAsync(string id);

        /// <summary>
        /// Delete entry by where filter.
        /// </summary>
        /// <param name="filters">the filter to find the entry</param>
        /// <returns></returns>
        (bool succeed, TError error) DeleteOne(Expression<Func<T, bool>> filters);

        /// <summary>
        /// Delete entry by where filter [ Async Version ].
        /// </summary>
        /// <param name="filters">the filter to find the entry</param>
        /// <returns></returns>
        ValueTask<(bool succeed, TError error)> DeleteOneAsync(Expression<Func<T, bool>> filters);

        /// <summary>
        /// Delete entries by where filter.
        /// </summary>
        /// <param name="filters">the filter to find the entries</param>
        /// <returns></returns>
        (bool succeed, TError error) DeleteAll(Expression<Func<T, bool>> filters);

        /// <summary>
        /// Delete entries by where filter [ Async Version ].
        /// </summary>
        /// <param name="filters">the filter to find the entries</param>
        /// <returns></returns>
        ValueTask<(bool succeed, TError error)> DeleteAllAsync(Expression<Func<T, bool>> filters);

    }

}
