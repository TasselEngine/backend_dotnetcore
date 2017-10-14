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

        (string entry_id, bool succeed, TError error) UpdateOne(T entry, string id);

        ValueTask<(string entry_id, bool succeed, TError error)> UpdateOneAsync(T entry, string id);

        (IEnumerable<T> collection, bool succeed, TError error) GetCollections(Expression<Func<T, bool>> where = null, int? skip = null, int? take = null);

        ValueTask<(IEnumerable<T> collection, bool succeed, TError error)> GetCollectionsAsync(Expression<Func<T, bool>> where = null, int? skip = null, int? take = null);

        (T entry, bool succeed, TError error) FindOneByID(string id);

        ValueTask<(T entry, bool succeed, TError error)> FindOneByIDAsync(string id);

    }
}
