using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Model.Utils;
using Tassel.Services.Contract.Providers;

namespace Tassel.Services.Contract {

    public interface ILikeableService<T,TError, TEntry> : ILogicalDeleteService<T, TError> where T : BaseLikesModel where TEntry : ILikeable<BaseCreator> {

        ILikesServiceProvider Likes { get; }

        ValueTask<(string user_id, JsonStatus status, Error error)> LikeAsync(string id, TEntry like);

    }

}
