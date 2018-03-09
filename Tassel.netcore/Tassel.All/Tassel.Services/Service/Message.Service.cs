using BWS.Utils.NetCore.Expressions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Model.Utils;
using Tassel.Services.Contract;

namespace Tassel.Services.Service {

    public class MessageService : BaseService<Message>, IMessageService {

        public MessageService(MongoDBContext db) : base(db, ModelCollectionName.Message) { }

        protected UpdateDefinition<Message> DefineReadMessageUpdate(Message entry) {
            var define = base.DefineUpdate(entry);
            define.Set(i => i.ReadState, ReadType.Read);
            return define;
        }

        public async ValueTask<(JsonStatus status, Error error)> CreateMessageAsync(
            BaseCreator creator, BaseCreator target, MessageType type, string content, MessageSource source) {
            var message = new Message {
                Creator = creator,
                Content = content,
                Receiver = target,
                MessageType = type,
                ReadState = ReadType.Unread,
                Source = source
            };
            try {
                await this.collection.InsertOneAsync(message);
                return (JsonStatus.Succeed, Error.Empty);
            } catch (Exception e) {
                return (JsonStatus.MessageCreateFailed, Error.Create(Errors.MessageCreateFailed, e.Message));
            }
        }

        public async ValueTask<(IEnumerable<Message> msgs, JsonStatus status, Error error)> FetchMessagesAsync(
            string uuid,
            long? before,
            long? after,
            int take,
            bool? unread = null) {

            try {
                Expression<Func<Message, bool>> where = i => true;
                if (before.HasValue)
                    where = where.And(m => m.CreateTime < before.GetValueOrDefault());
                if (after.HasValue)
                    where = where.And(a => a.CreateTime > after.GetValueOrDefault());
                if (unread.HasValue)
                    where = where.And(b => b.ReadState == (unread.Value ? ReadType.Unread : ReadType.Read));
                var (coll, succeed, error) = await this.GetCollectionsAsync(where, take: take);
                if (!succeed)
                    return (null, JsonStatus.GetMessagesFailed, error);
                return (coll, JsonStatus.Succeed, Error.Empty);
            } catch (Exception e) {
                return (null, JsonStatus.GetMessagesFailed, Error.Create(Errors.GetMessagesFailed, e.Message));
            }
        }

        public async ValueTask<(long count, JsonStatus status, Error error)> ReadMessagesAsync(string[] ids) {
            try {
                var (count, succeed, error) = await this.UpdateManyAsync(ids, null, this.DefineReadMessageUpdate(null));
                if (succeed)
                    return (count, JsonStatus.Succeed, Error.Empty);
                return (count, JsonStatus.ReadMessageFailed, error);
            } catch (Exception e) {
                return (0, JsonStatus.GetMessagesFailed, Error.Create(Errors.ReadMessagesFailed, e.Message));
            }
        }
    }
}
