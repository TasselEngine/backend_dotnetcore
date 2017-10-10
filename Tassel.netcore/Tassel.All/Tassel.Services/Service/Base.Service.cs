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
using Tassel.Model.Models.BsonModels;

namespace Tassel.Services.Service {
    public abstract class BaseService<T> : IBusinessService<T, Error> where T : BaseModel {

        protected IMongoDatabase mdb;
        protected IMongoCollection<T> collection;

        public BaseService(MongoDBContext db, string collName) {
            this.mdb = db.DB;
            this.collection = mdb.GetCollection<T>(collName);
        }

        public (T entry, bool succeed, Error error) FindOneByID(string id) {
            try {
                var entry = this.collection.Find(i => i.ID == id).FirstOrDefault();
                return (entry, true, Error.Empty);
            } catch (Exception e) {
                return (default(T), false, Error.Create(Errors.QueryEntryFailed, e.Message));
            }
        }

        public async ValueTask<(T entry, bool succeed, Error error)> FindOneByIDAsync(string id) {
            try {
                var entry = (await this.collection.FindAsync(i => i.ID == id)).FirstOrDefault();
                return (entry, true, Error.Empty);
            } catch (Exception e) {
                return (default(T), false, Error.Create(Errors.QueryEntryFailed, e.Message));
            }
        }

        public (IEnumerable<T> collection, bool succeed, Error error) GetCollections(
            Expression<Func<T, bool>> where = null, 
            int? skip = null, int? 
            take = null) {

            where = where ?? (i => true);
            try {
                var coll = this.collection.Find(where).ToEnumerable();
                if (skip != null)
                    coll = coll.Skip(skip.GetValueOrDefault());
                if (take != null)
                    coll = coll.Take(take.GetValueOrDefault());
                return (coll, true, Error.Empty);
            } catch (Exception e) {
                return (default(IEnumerable<T>), false, Error.Create(Errors.GetEntryCollFailed, e.Message));
            }
        }

        public async ValueTask<(IEnumerable<T> collection, bool succeed, Error error)> GetCollectionsAsync(
            Expression<Func<T, bool>> where = null, 
            int? skip = null, int? 
            take = null) {

            where = where ?? (i => true);
            try {
                var coll = (await this.collection.FindAsync(where)).ToEnumerable();
                if (skip != null)
                    coll = coll.Skip(skip.GetValueOrDefault());
                if (take != null)
                    coll = coll.Take(take.GetValueOrDefault());
                return (coll, true, Error.Empty);
            } catch (Exception e) {
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
            } catch (Exception e) {
                return (default(T), false, Error.Create(Errors.InsertOneFailed, e.Message));
            }
        }

    }

}
