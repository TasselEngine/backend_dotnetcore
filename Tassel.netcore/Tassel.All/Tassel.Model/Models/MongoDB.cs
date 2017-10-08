using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tassel.Model.Models {
    public class MongoDBContext {

        public IMongoDatabase DB { get; set; }

        public MongoDBContext(IConfiguration config) {
            var dbcon = config.GetSection("MongoDBStrings");
            var client = new MongoClient(dbcon["DefaultConnection"]);
            DB = client.GetDatabase(dbcon["DBName"]);
        }

    }
}
