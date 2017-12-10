using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Model.Utils;
using Tassel.Services.Contract.Providers;
using Tassel.Services.Service;

namespace Tassel.Services.Providers {
    public class CommentProvider : BaseService<Comment>, ICommentServiceProvider {

        public CommentProvider(MongoDBContext db) : base(db, ModelCollectionName.Comment) { }

        protected UpdateDefinition<Comment> DefineCommentsUpdate(Comment comment, bool add = true) {
            var def = Builders<Comment>.Update;
            return add ?
                def.Push(i => i.Comments, comment) :
                def.PullFilter(i => i.Comments, c => c.ID == comment.ID && c.Creator.UUID == comment.Creator.UUID);
        }

        public async ValueTask<(JsonStatus status, Error error)> AddReplyForCommentAsync(string com_id, Comment model) {
            var (entry, succeed, error) = await this.FindOneUpdateAsync(com_id, null, this.DefineCommentsUpdate(model));
            if (!succeed)
                return (JsonStatus.CommentAddFailed, error);
            if (entry == null)
                return (JsonStatus.CommentAddFailed, Error.Create(Errors.EntryNotExist));
            return (JsonStatus.Succeed, Error.Empty);
        }

        public async ValueTask<(JsonStatus status, Error error)> RemoveReplyForCommentAsync(string com_id, string reply_id, string uid) {
            var (entry, succeed, error) = await this.FindOneUpdateAsync(com_id, null, this.DefineCommentsUpdate(
                new Comment { ID = reply_id, Creator = new BaseCreator { UUID = uid } }, false));
            if (!succeed)
                return (JsonStatus.CommentRemoveFailed, error);
            if (entry == null)
                return (JsonStatus.CommentRemoveFailed, Error.Create(Errors.EntryNotExist));
            var item = entry.Comments.FirstOrDefault(i => i.ID == reply_id);
            if (item == null || item.Creator == null || item.Creator.UUID != uid)
                return (JsonStatus.CommentRemoveFailed, Error.Create(Errors.DeleteEntryFailed, Errors.AccessDenied));
            return (JsonStatus.Succeed, Error.Empty);
        }
    }
}
