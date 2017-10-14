using System;
using System.Collections.Generic;
using System.Text;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Services.Contract.Providers;
using Tassel.Services.Service;

namespace Tassel.Services.Providers {
    public class CommentProvider : BaseService<Comment>, ICommentServiceProvider<Comment> {

        public CommentProvider(MongoDBContext db) : base(db, ModelCollectionName.Comment) { }

    }
}
