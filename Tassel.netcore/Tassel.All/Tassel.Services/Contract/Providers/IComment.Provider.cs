using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Model.Utils;

namespace Tassel.Services.Contract.Providers {

    public interface ICommentServiceProvider<T> : IBusinessService<T, Error> where T : BaseModel {

        ValueTask<(JsonStatus status, Error error)> AddReplyForCommentAsync(string com_id, Comment model);

        ValueTask<(JsonStatus status, Error error)> RemoveReplyForCommentAsync(string com_id,  string reply_id, string uid);

    }

}
