using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tassel.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;

namespace Tassel.Service.Utils.Extensions {
    public static class DbExtensions { 

        public static void AddApplicationDbContext(this IServiceCollection services, IConfiguration Configuration)
            => services.AddDbContext<APIDB>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    bOptions => bOptions.MigrationsAssembly("Tassel.Model")));

        public static void DbSeedDataInsert(this IApplicationBuilder app) => DbHelper.SetSeedData(app.ApplicationServices);

    }
}