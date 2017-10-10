using System;
using System.Collections.Generic;
using System.Text;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Services.Contract;

namespace Tassel.Services.Service
{
    public class CommentService : BaseService<Comment>, ICommentService {

        public CommentService(MongoDBContext db) :base(db, ModelCollectionName.Comment) { }

    }
}
