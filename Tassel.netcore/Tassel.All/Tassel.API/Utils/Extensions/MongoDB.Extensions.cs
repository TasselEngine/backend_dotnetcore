using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tassel.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tassel.API.Utils.Extensions {

    public static class MongoDBExtensions {

        public static void AddMongoDbContext(this IServiceCollection services, IConfiguration config) {
            services.AddScoped(provider => new MongoDBContext(config));
        }
    }

}
