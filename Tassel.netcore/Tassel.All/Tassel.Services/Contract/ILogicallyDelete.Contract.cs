using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Models.BsonModels;

namespace Tassel.Services.Contract {

    /// <summary>
    /// Support Tag-Delete feature for page entry base with this interface.
    /// </summary>
    /// <typeparam name="T">type extends by DeleteSafelyBase</typeparam>
    /// <typeparam name="TError">type of the error to be returned</typeparam>
    public interface ILogicalDeleteService<T, TError> : IBusinessService<T, TError> where T : IAccessControllableBase {

        /// <summary>
        /// Delete entry safely (tag-delete) by id.
        /// </summary>
        /// <param name="entry_id">key</param>
        /// <returns></returns>
        (bool succeed, TError error) SafeDeleteOneByID(string entry_id);

        /// <summary>
        /// Delete entry safely (tag-delete) by id [ Async Version ].
        /// </summary>
        /// <param name="entry_id">key</param>
        /// <returns></returns>
        ValueTask<(bool succeed, TError error)> SafeDeleteOneByIDAsync(string entry_id);

        /// <summary>
        /// Delete entry safely (tag-delete) by where filter.
        /// </summary>
        /// <param name="filters">where filter</param>
        /// <returns></returns>
        (bool succeed, TError error) SafeDeleteOneByFilter(Expression<Func<T, bool>> filters);

        /// <summary>
        /// Delete entry safely (tag-delete) by where filter [ Async Version ].
        /// </summary>
        /// <param name="filters">where filter</param>
        /// <returns></returns>
        ValueTask<(bool succeed, TError error)> SafeDeleteOneByFilterAsync(Expression<Func<T, bool>> filters);

        /// <summary>
        /// Get the collections are published (in tag-delete mode entries coll) where the filter is passed with skip and take params.
        /// </summary>
        /// <param name="where">filter</param>
        /// <param name="skip">number to skip</param>
        /// <param name="take">nnumber to take</param>
        /// <returns></returns>
        (IList<T> collection, bool succeed, TError error) GetPublishedCollections(Expression<Func<T, bool>> where = null, int? skip = null, int? take = null);

        /// <summary>
        /// Get the collections are published (in tag-delete mode entries coll) where the filter is passed with skip and take params [ Async Version ].
        /// </summary>
        /// <param name="stamp">stamp</param>
        /// <param name="take">nnumber to take</param>
        /// <returns></returns>
        ValueTask<(IList<T> collection, bool succeed, TError error)> GetPublishedCollectionsAsync(long? before, int? take = null);

    }
}
