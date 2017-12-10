using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Model.Utils;

namespace Tassel.Services.Contract {

    public interface ILikeableService<T,TError, TEntry> : ILogicalDeleteService<T, TError> where T : IBaseLikeModel where TEntry : ILikeable {

        ValueTask<(string user_id, JsonStatus status, Error error)> LikeAsync(string id, TEntry like);

    }

}
