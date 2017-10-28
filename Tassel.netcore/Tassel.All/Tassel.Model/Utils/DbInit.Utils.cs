using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;

namespace Tassel.Model.Utils {
    public static class MongoDBExtensions {

        public static void CreateMongoSeed(this IApplicationBuilder app, IConfiguration config) {

            var factory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using(var scope = factory.CreateScope()) {
                var mongo = scope.ServiceProvider.GetRequiredService<MongoDBContext>();
                var dbcon = config.GetSection("MongoDBStrings");
                var DB = new MongoClient(dbcon["DefaultConnection"]).GetDatabase(dbcon["DBName"]);
                var users = DB.GetCollection<User>(ModelCollectionName.User);
                if (users.Count(i => i.UserName == "admin") == 0) {
                    users.InsertOne(IdentityProvider.CreateAdmin("admin", "123456"));
                }
            }

        }

    }
}
