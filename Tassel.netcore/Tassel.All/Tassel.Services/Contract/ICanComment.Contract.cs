using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Model.Utils;
using Tassel.Services.Contract.Providers;

namespace Tassel.Services.Contract {

    public interface ICommentableService<T, TError, TEntry> : ILogicalDeleteService<T, TError> where T : CanCommentModel where TEntry : IComment<BaseCreator> {

        ICommentServiceProvider Comments { get; }

        ValueTask<(JsonStatus status, Error error)> AddCommentAsync(string id, TEntry comment);

        ValueTask<(JsonStatus status, Error error)> RemoveCommentAsync(string id, string uid, string comment_id);

    }

}

