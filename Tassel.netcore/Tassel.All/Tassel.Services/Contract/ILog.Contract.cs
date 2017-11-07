using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Models.BsonModels;
using Tassel.Model.Utils;

namespace Tassel.Services.Contract {
    public interface ILogService : IBusinessService<LogEntry, Error> {

        ValueTask<(bool succeed, Error error)> Debug(Agent agent, Target target, ModelType targetType, LogAction action, string role = null);

        ValueTask<(bool succeed, Error error)> Info(Agent agent, Target target, ModelType targetType, LogAction action, string role = null);

        ValueTask<(bool succeed, Error error)> Warn(Agent agent, Target target, ModelType targetType, LogAction action, string role = null);

        ValueTask<(bool succeed, Error error)> Error(Agent agent, Target target, ModelType targetType, LogAction action, string role = null);

        ValueTask<(bool succeed, Error error)> Fatal(Agent agent, Target target, ModelType targetType, LogAction action, string role = null);

    }
}
