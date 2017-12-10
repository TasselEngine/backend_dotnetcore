using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Model.Utils;

namespace Tassel.Services.Contract {

    public interface IStatusService : ILikeableService<Status, Error, LikesEntry>, ICommentableService<Status, Error, Comment> {

        ValueTask<(IEnumerable<Status> entry, JsonStatus status, Error error)> GetCollectionAbstractAsync(Expression<Func<Status, bool>> where = null);

        ValueTask<(IEnumerable<Status> entry, JsonStatus status, Error error)> GetCollectionAbstractAsync(long stamp, int? take);

        ValueTask<(Status entry, JsonStatus status, Error error)> GetStatusDetailsAsync(string id);

        ValueTask<(Status entry, JsonStatus status, Error error)> GetStatusAbstractAsync(string id);

        ValueTask<(JsonStatus status, Error error)> DeleteStatusAsync(string id);

    }

}
