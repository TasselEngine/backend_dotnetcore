using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Model.Utils;

namespace Tassel.Services.Contract {

    public interface IPost : ILogicalDeleteService<Post, Error> {

        ValueTask<(Post entry, JsonStatus status, Error error)> GetPostDetailsAsync(string id);

        ValueTask<(Post entry, JsonStatus status, Error error)> GetPostDetailsByKeyAsync(string plain_key);

    }

}
