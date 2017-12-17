using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Services.Contract.Providers;

namespace Tassel.Services.Contract.Components {

    public interface ICommentableServiceComponent<T, TError, TEntry> where T : CanCommentModel where TEntry : IComment<BaseCreator> {

        ICommentServiceProvider Comments { get; }

        ValueTask<(JsonStatus status, TError error)> AddCommentAsync(string id, TEntry comment);

        ValueTask<(JsonStatus status, TError error)> RemoveCommentAsync(string id, string uid, string comment_id);

    }

}
