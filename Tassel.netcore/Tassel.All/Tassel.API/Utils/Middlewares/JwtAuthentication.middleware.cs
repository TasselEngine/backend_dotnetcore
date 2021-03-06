﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Tassel.Model.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Tassel.Services.Contract;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using BWS.Utils.NetCore.Format;
using Tassel.API.VM.Token;
using Tassel.API.VM.Identity;
using Tassel.Service.Utils.Helpers;
using Tassel.Services.Utils.Constants;
using BWS.Utils.NetCore.Seriailzation.ContractResolvers;
using Tassel.Model.Models.BsonModels;

namespace Tassel.Service.Utils.Middlewares {

    public static class TokenProviderExtensions {
        public static IApplicationBuilder AddTasselTokenCreator(
            this IApplicationBuilder builder,
            TokenProviderOptions opts) => builder.UseMiddleware<TokenCreatorMiddleware>(Options.Create(opts));
    }

    enum ProviderType { Login, Register, Weibo, Undefined }

    class PostParams {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string WeiboUid { get; set; }
    }

    public class TokenCreatorMiddleware {

        private readonly RequestDelegate skip;
        private readonly TokenProviderOptions opts;
        private IIdentityService<JwtSecurityToken, TokenProviderOptions, User> identity;
        private IServiceProvider serviceProvider;

        public TokenCreatorMiddleware(
            IServiceProvider serviceProvider,
            RequestDelegate next,
            IOptions<TokenProviderOptions> options) {
            this.serviceProvider = serviceProvider;
            skip = next;
            opts = options.Value;
        }

        public Task Invoke(HttpContext context) {

            var type =
                context.Request.Path.Equals(opts.RegisterPath, StringComparison.Ordinal) ? ProviderType.Register :
                context.Request.Path.Equals(opts.LoginPath, StringComparison.Ordinal) ? ProviderType.Login :
                context.Request.Path.Equals(opts.WeiboCheckPath, StringComparison.Ordinal) ? ProviderType.Weibo :
                ProviderType.Undefined;

            if (type == ProviderType.Undefined)
                return skip(context);
            if (!context.Request.Method.Equals("POST"))
                return skip(context);
            if (!context.Request.HasFormContentType && context.Request.ContentType != "application/json")
                return skip(context);

            return GenerateToken(context, new JsonBase(), type);
        }

        private async Task<string> GetContentAsync(HttpContext context) {
            var bts = new byte[context.Request.ContentLength.Value];
            await context.Request.Body.ReadAsync(bts, 0, bts.Length);
            return Encoding.UTF8.GetString(bts);
        }

        private async Task GenerateToken(HttpContext context, JsonBase model, ProviderType type) {

            var param = new PostParams();

            if (context.Request.ContentType != "application/json") {
                if (type == ProviderType.Weibo) {
                    param.WeiboUid = context.Request.Form["wuid"];
                } else {
                    param.UserName = context.Request.Form["user"];
                    param.Password = context.Request.Form["psd"];
                }
            } else {
                var user = JsonHelper.FromJson<JwtProviderParam>(await GetContentAsync(context));
                if (type == ProviderType.Weibo) {
                    param.WeiboUid = user.WeiboUID;
                } else {
                    param.UserName = user.UserName;
                    param.Password = user.Password;
                }
            }

            context.Response.ContentType = "application/json";

            var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope()) {

                this.identity = scope.ServiceProvider.GetRequiredService<IIdentityService<JwtSecurityToken, TokenProviderOptions, User>>();

                var (user, error) = GetIdentity(param, type);
                if (error != null) {
                    model.Message = error;
                    model.Status = type == ProviderType.Register ? JsonStatus.RegisterFailed : JsonStatus.LoginFailed;
                    model.Content = new {
                        RedirectUrl = context.Request.Path
                    };
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(model, new JsonSerializerSettings {
                        ContractResolver = new LowercaseContractResolver(),
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        Formatting = Formatting.Indented
                    }));
                    return;
                }

                model.Status = JsonStatus.Succeed;
                model.Message = null;

                var vmProvider = new UserVM(user);
                if(type == ProviderType.Weibo) {
                    vmProvider = vmProvider.Create(this.identity.WeiboService.SearchWeiboUserInfoByUID);
                }

                model.Content = new TokenProviderVM {
                    Token = new JwtSecurityTokenHandler().WriteToken(identity.GenerateToken(user, opts)),
                    Expires = (int)opts.Expiration.TotalSeconds,
                    Details = vmProvider.User
                };

                await context.Response.WriteAsync(JsonConvert.SerializeObject(model, new JsonSerializerSettings {
                    ContractResolver = new LowercaseContractResolver(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented
                }));

            }

        }

        private (User, string) GetIdentity(PostParams param, ProviderType type) {
            var (user, ok, error) =
                type == ProviderType.Register ? identity.TryRegister(param.UserName, param.Password) :
                type == ProviderType.Login ? identity.TryLogin(param.UserName, param.Password) :
                type == ProviderType.Weibo ? identity.WeiboService.TryGetUserByWeibo(param.WeiboUid) :
                (null, false, "failed");
            if (ok)
                return (user, null);
            return (null, error);
        }

    }
}
