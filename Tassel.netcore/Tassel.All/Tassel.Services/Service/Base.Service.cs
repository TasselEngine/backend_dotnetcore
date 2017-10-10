using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Utils;
using Tassel.Services.Contract;
using System.Linq.Expressions;
using System.Linq;
using Tassel.Model.Models;

namespace Tassel.Services.Service
{
    public abstract class BaseService<T> : IBusinessService<T, Error> {

        protected IMongoDatabase mdb;
        protected IMongoCollection<T> collection;

        public BaseService(MongoDBContext db, string collName) {
            this.mdb = db.DB;
            this.collection = mdb.GetCollection<T>(collName);
        }

        public (IEnumerable<T> collection, bool succeed, Error error) GetCollections(Expression<Func<T, bool>> where = null) {
            where = where ?? (i => true);
            try {
                var coll = this.collection.AsQueryable().Where(where).AsEnumerable();
                return (coll, true, Error.Empty);
            } catch(Exception e) {
                return (default(IEnumerable<T>), false, Error.Create(Errors.GetEntryCollFailed, e.Message));
            }
        }

        public (T entry, bool succeed, Error error) InsertOne(T entry) {
            try {
                this.collection.InsertOne(entry);
                return (entry, true, Error.Empty);
            } catch (Exception e) {
                return (default(T), false, Error.Create(Errors.InsertOneFailed, e.Message));
            }
        }

        public async ValueTask<(T entry, bool succeed, Error error)> InsertOneAsync(T entry) {
            try {
                await this.collection.InsertOneAsync(entry);
                return (entry, true, Error.Empty);
            }catch(Exception e) {
                return (default(T), false, Error.Create(Errors.InsertOneFailed, e.Message));
            }
        }

    }

}
