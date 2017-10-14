using System;
using System.Collections.Generic;
using System.Text;
using Tassel.Model.Models.BsonModels;
using Tassel.Model.Utils;

namespace Tassel.Services.Contract.Providers {

    public interface ICommentServiceProvider<T> : IBusinessService<T, Error> where T : BaseModel {

    }

}
