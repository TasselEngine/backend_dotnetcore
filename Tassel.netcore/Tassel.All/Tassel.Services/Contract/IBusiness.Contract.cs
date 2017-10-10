using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tassel.Services.Contract {
    public interface IBusinessService<T, TError> {

        (T entry, bool succeed, TError error) InsertOne(T entry);

        ValueTask<(T entry, bool succeed, TError error)> InsertOneAsync(T entry);

        (IEnumerable<T> collection, bool succeed, TError error) GetCollections(Expression<Func<T, bool>> where = null);

    }
}
