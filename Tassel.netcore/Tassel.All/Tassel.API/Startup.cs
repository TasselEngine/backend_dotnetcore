using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Tassel.API.Utils.Extensions;
using Tassel.Service.Utils.Middlewares;
using Tassel.Services.Contract;
using System.IdentityModel.Tokens.Jwt;
using Tassel.Services.Service;
using WeiboOAuth2.Provider;
using Tassel.API.Utils.Helpers;
using Tassel.Services.Utils.Constants;
using Tassel.Service.Utils.Helpers;
using Tassel.Model.Models.BsonModels;
using Tassel.Services.Contract.Providers;
using Tassel.Services.Providers;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Tassel.Model.Utils;
using Tassel.Services.Components;
using Microsoft.AspNetCore.Mvc;

namespace Tassel.Service {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            services.AddMongoDbContext(Configuration);

            services.AddScoped<IStaticService, StaticService>();
            services.AddScoped<IWeiboOAuthV2Option, WeiboOAuthV2Option>();
            services.AddScoped<IWeiboOAuthServiceProvider<User>, WeiboOAuthProvider>();
            services.AddScoped<IIdentityService<JwtSecurityToken, TokenProviderOptions, User>, IdentityService>();
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<ICommentServiceProvider, CommentProvider>();
            services.AddScoped<ILikesServiceProvider, LikesService>();
            services.AddScoped<ILogService, LogService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddTasselJwtBearer(options => {
                    options.RequireHttpsMetadata = false;
                    options.IncludeErrorDetails = true;
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = TokenProvider.CreateKey(Configuration),
                        ValidIssuer = TokenProviderEntry.Issuer,
                        ValidAudience = TokenProviderEntry.Audience,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddMvc(option=> {
                option.UseCentralRoutePrefix(new RouteAttribute("api/v{version}"));
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
            app.UseStaticFiles(new StaticFileOptions {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "resources")),
                RequestPath = new PathString("/static/resources")
            });
            app.UseMvc();

            app.AddTasselTokenCreator(new TokenProviderOptions {
                Audience = TokenProviderEntry.Audience,
                Issuer = TokenProviderEntry.Issuer,
                LoginPath = "/api/v1/" + TokenProviderEntry.LoginPath,
                RegisterPath = "/api/v1/" + TokenProviderEntry.RegisterPath,
                WeiboCheckPath = "/api/v1/" + TokenProviderEntry.WeiboCheckPath,
                SigningCredentials = new SigningCredentials(TokenProvider.CreateKey(Configuration), SecurityAlgorithms.HmacSha256),
            });

            app.CreateMongoSeed(Configuration);

        }
    }
}
