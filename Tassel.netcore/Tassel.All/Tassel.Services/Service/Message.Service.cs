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

        public async ValueTask<(IEnumerable<Message> msgs, JsonStatus status, Error error)> FetchMessagesAsync(string uuid, long? before, int take, bool? unread = null) {
            //Expression<Func<Message, bool>> where = i => i.Receiver.UUID == uuid;
            //if (unread != null) 
            //    where.And(i => i.ReadState == (unread.GetValueOrDefault() ? ReadType.Unread : ReadType.Read));
            Expression<Func<Message, bool>> where = i => true;
            var (coll, succeed, error) = default((IList<Message>, bool, Error));
            if (before.HasValue) {
                (coll, succeed, error) = await this.GetCollectionsAsync(before.GetValueOrDefault(), take, where);
            } else {
                (coll, succeed, error) = this.GetCollections(where, take: take);
            }
            if (!succeed)
                return (null, JsonStatus.GetMessagesFailed, error);
            return (coll, JsonStatus.Succeed, Error.Empty);
        }
    }
}
