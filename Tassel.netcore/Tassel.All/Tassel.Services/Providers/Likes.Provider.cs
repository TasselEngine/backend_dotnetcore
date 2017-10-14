using System;
using System.Collections.Generic;
using System.Text;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Services.Contract;
using Tassel.Services.Contract.Providers;
using Tassel.Services.Service;

namespace Tassel.Services.Providers {

    public class LikesService : BaseService<LikesEntry>, ILikesServiceProvider {

        public LikesService(MongoDBContext db) :base(db, ModelCollectionName.Likes) { }

    }

}
