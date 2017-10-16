using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Models.BsonModels;

namespace Tassel.Services.Contract {
    public interface IBusinessService<T, TError> where T : BaseModel {

        (T entry, bool succeed, TError error) InsertOne(T entry);

        ValueTask<(T entry, bool succeed, TError error)> InsertOneAsync(T entry);

        (string entry_id, bool succeed, TError error) UpdateOne(string id, T toDo = null, UpdateDefinition<T> updateDef = null);

        ValueTask<(string entry_id, bool succeed, TError error)> UpdateOneAsync(string id, T toDo = null, UpdateDefinition<T> updateDef = null);

        (T outEntry, bool succeed, TError error) FindOneUpdate(string id, T toDo = null, UpdateDefinition<T> updateDef = null);

        ValueTask<(T outEntry, bool succeed, TError error)> FindOneUpdateAsync(string id, T toDo = null, UpdateDefinition<T> updateDef = null);

        (IList<T> collection, bool succeed, TError error) GetCollections(Expression<Func<T, bool>> where = null, int? skip = null, int? take = null);

        ValueTask<(IList<T> collection, bool succeed, TError error)> GetCollectionsAsync(Expression<Func<T, bool>> where = null, int? skip = null, int? take = null);

        (T entry, bool succeed, TError error) FindOneByID(string id);

        ValueTask<(T entry, bool succeed, TError error)> FindOneByIDAsync(string id);

        (bool succeed, TError error) DeleteOneByID(string entry_id);

        ValueTask<(bool succeed, TError error)> DeleteOneByIDAsync(string entry_id);

        (bool succeed, TError error) DeleteOneByFilter(Expression<Func<T, bool>> filters = null);

        ValueTask<(bool succeed, TError error)> DeleteOneByFilterAsync(Expression<Func<T, bool>> filters = null);

    }
}
