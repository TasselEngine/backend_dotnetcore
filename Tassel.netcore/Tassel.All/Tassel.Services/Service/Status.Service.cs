using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Model.Utils;
using Tassel.Services.Contract;

namespace Tassel.Services.Service {

    public class StatusService : BaseService<Status>, IStatusService {

        public StatusService(MongoDBContext db) :base(db, ModelCollectionName.Status) { }

    }

}
