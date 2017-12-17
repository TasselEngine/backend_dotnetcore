using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Model.Utils;
using Tassel.Services.Contract;
using Tassel.Services.Contract.Components;
using Tassel.Services.Contract.Providers;

namespace Tassel.Services.Components {

    public interface ICommentComponent<T> : ICommentableServiceComponent<T, Error, Comment> where T : CanCommentModel { }

    public class CommentableServiceComponent<T> : ICommentComponent<T> where T : CanCommentModel {

        private ILogicalDeleteService<T, Error> context;
        private ICommentServiceProvider comments;

        public ICommentServiceProvider Comments => this.comments;

        public CommentableServiceComponent(ILogicalDeleteService<T, Error> context, ICommentServiceProvider comments) {
            this.context = context;
            this.comments = comments;
        }

        protected UpdateDefinition<T> DefineCommentsUpdate(string comment_id, bool add = true) {
            var def = Builders<T>.Update;
            return add ?
                def.Push(i => i.CommentIDs, comment_id) :
                def.Pull(i => i.CommentIDs, comment_id);
        }

        public async ValueTask<(JsonStatus status, Error error)> AddCommentAsync(string id, Comment comment) {
            var (entry, succeed, error) = await this.context.FindOneUpdateAsync(id, null, this.DefineCommentsUpdate(comment.ID));
            if (!succeed)
                return (JsonStatus.CommentAddFailed, error);
            if (entry == null)
                return (JsonStatus.CommentAddFailed, Error.Create(Errors.EntryNotExist));
            (_, succeed, error) = await this.comments.InsertOneAsync(comment);
            if (!succeed) {
                // Rollback, anyway
                await this.context.FindOneUpdateAsync(id, null, this.DefineCommentsUpdate(comment.ID, false));
                return (JsonStatus.CommentAddFailed, error);
            }
            return (JsonStatus.Succeed, Error.Empty);
        }

        public async ValueTask<(JsonStatus status, Error error)> RemoveCommentAsync(string id, string uid, string comment_id) {
            var (entry, succeed, error) = await this.context.FindOneUpdateAsync(id, null, this.DefineCommentsUpdate(comment_id, false));
            if (!succeed)
                return (JsonStatus.CommentRemoveFailed, error);
            if (entry == null)
                return (JsonStatus.CommentRemoveFailed, Error.Create(Errors.EntryNotExist));
            (succeed, error) = await this.comments.DeleteOneAsync(i => i.ID == comment_id && i.Creator.UUID == uid && i.ParentID == id);
            if (!succeed) {
                // Rollback, anyway
                await this.context.FindOneUpdateAsync(id, null, this.DefineCommentsUpdate(comment_id));
                return (JsonStatus.CommentRemoveFailed, error);
            }
            return (JsonStatus.Succeed, Error.Empty);
        }

    }

}
