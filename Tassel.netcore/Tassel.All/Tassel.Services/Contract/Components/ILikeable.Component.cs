using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Services.Contract.Providers;

namespace Tassel.Services.Contract.Components {

    public interface ILikeableServiceComponent<T, TError, TEntry> where T : BaseLikesModel where TEntry : ILikeable<BaseCreator> {

        ILikesServiceProvider Likes { get; }

        ValueTask<(string user_id, JsonStatus status, TError error)> LikeAsync(string id, TEntry like);

    }

}
