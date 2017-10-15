﻿using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Models.BsonModels;

namespace Tassel.Services.Contract {
    public interface IBusinessService<T, TError> where T : BaseModel {

        (T entry, bool succeed, TError error) InsertOne(T entry);

        ValueTask<(T entry, bool succeed, TError error)> InsertOneAsync(T entry);

        (string entry_id, bool succeed, TError error) UpdateOne(T entry, string id, UpdateDefinition<T> updateDef = null);

        ValueTask<(string entry_id, bool succeed, TError error)> UpdateOneAsync(T entry, string id, UpdateDefinition<T> updateDef = null);

        (T outEntry, bool succeed, TError error) FindOneUpdate(T entry, string id, UpdateDefinition<T> updateDef = null);

        ValueTask<(T outEntry, bool succeed, TError error)> FindOneUpdateAsync(T entry, string id, UpdateDefinition<T> updateDef = null);

        (IList<T> collection, bool succeed, TError error) GetCollections(Expression<Func<T, bool>> where = null, int? skip = null, int? take = null);

        ValueTask<(IList<T> collection, bool succeed, TError error)> GetCollectionsAsync(Expression<Func<T, bool>> where = null, int? skip = null, int? take = null);

        (T entry, bool succeed, TError error) FindOneByID(string id);

        ValueTask<(T entry, bool succeed, TError error)> FindOneByIDAsync(string id);

        (bool succeed, TError error) DeleteOne(string entry_id);

        ValueTask<(bool succeed, TError error)> DeleteOneAsync(string entry_id);

    }
}
