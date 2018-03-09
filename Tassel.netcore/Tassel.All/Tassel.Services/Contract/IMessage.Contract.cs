using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Model.Utils;

namespace Tassel.Services.Contract {
    public interface IMessageService : IBusinessService<Message, Error>{

        ValueTask<(JsonStatus status, Error error)> CreateMessageAsync(BaseCreator creator, BaseCreator target, MessageType type, string content, MessageSource source);

        ValueTask<(IEnumerable<Message> msgs, JsonStatus status, Error error)> FetchMessagesAsync(string uuid, long? before, long? after, int take, bool? unread = null);

        ValueTask<(long count, JsonStatus status, Error error)> ReadMessagesAsync(string[] ids);

    }
}
