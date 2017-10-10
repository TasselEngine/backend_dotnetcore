using System;
using System.Collections.Generic;
using System.Text;
using Tassel.Model.Models.BsonModels;
using Tassel.Model.Utils;
using Tassel.Services.Contract;

namespace Tassel.Services.Contract {

    public interface IStatusService : IBusinessService<Status, Error> {

    }

}
