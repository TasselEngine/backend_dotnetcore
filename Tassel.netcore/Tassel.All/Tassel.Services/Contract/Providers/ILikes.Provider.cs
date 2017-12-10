using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Models.BsonModels;
using Tassel.Model.Utils;

namespace Tassel.Services.Contract.Providers {

    public interface ILikesServiceProvider : IBusinessService<ILikeable, Error> {

        (bool succeed, Error error) DeleteAllByIDs(string target_id, string user_id);

        ValueTask<(bool succeed, Error error)> DeleteAllByIDsAsync(string target_id, string user_id);

    }

}

