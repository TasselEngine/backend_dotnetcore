using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Model.Utils;
using Tassel.Services.Contract;

namespace Tassel.Services.Service {
    public class LogService : BaseService<LogEntry>, ILogService {

        public LogService(MongoDBContext db) : base(db, ModelCollectionName.Log) { }

        private async ValueTask<(bool succeed, Error error)> Log(LogLevel level, Agent agent, Target target, string role, ModelType targetType, LogAction action) {
            try {
                var urole =
                    string.IsNullOrEmpty(role) || role == UserRoleConstants.User ? LogRole.User :
                    role == UserRoleConstants.Admin ? LogRole.Admin :
                    role == UserRoleConstants.CORE ? LogRole.Core :
                    LogRole.Error;
                if (urole == LogRole.Error)
                    return (false, Model.Utils.Error.Create(Errors.CreateLogFailed, Errors.LogRoleError));
                var log = new LogEntry {
                    Creator = new BaseCreator { UUID = agent.UID, UserName = agent.UserName, AvatarUrl = agent.Avatar },
                    TargetID = target.ID,
                    TargetKey = target.Key,
                    TargetType = targetType,
                    Role = urole,
                    Action = action,
                    Description = target.Description,
                    Level = level
                };
                await this.collection.InsertOneAsync(log);
                return (true, Model.Utils.Error.Empty);
            } catch (Exception e) {
                return (false, Model.Utils.Error.Create(Errors.CreateLogFailed, e.Message));
            }
        }

        public async ValueTask<(bool succeed, Error error)> Debug(Agent agent, Target target, ModelType targetType, LogAction action, string role = null) {
            return await this.Log(LogLevel.Debug, agent, target, role, targetType, action);
        }

        public async ValueTask<(bool succeed, Error error)> Error(Agent agent, Target target, ModelType targetType, LogAction action, string role = null) {
            return await this.Log(LogLevel.Error, agent, target, role, targetType, action);
        }

        public async ValueTask<(bool succeed, Error error)> Fatal(Agent agent, Target target, ModelType targetType, LogAction action, string role = null) {
            return await this.Log(LogLevel.Fatal, agent, target, role, targetType, action);
        }

        public async ValueTask<(bool succeed, Error error)> Info(Agent agent, Target target, ModelType targetType, LogAction action, string role = null) {
            return await this.Log(LogLevel.Info, agent, target, role, targetType, action);
        }

        public async ValueTask<(bool succeed, Error error)> Warn(Agent agent, Target target, ModelType targetType, LogAction action, string role = null) {
            return await this.Log(LogLevel.Warn, agent, target, role, targetType, action);
        }
    }
}
