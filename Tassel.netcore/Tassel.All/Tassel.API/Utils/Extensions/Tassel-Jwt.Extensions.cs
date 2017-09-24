using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tassel.API.Utils.Handlers;

namespace Tassel.API.Utils.Extensions {
    public static class TasselJwtExtensions {
        /// <summary>
        /// 添加自定义的JWT处理程序 <see cref="TasselJwtHandler"/> 的扩展方法
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddTasselJwtBearer(this AuthenticationBuilder builder)
            => builder.AddTasselJwtBearer(JwtBearerDefaults.AuthenticationScheme, _ => { });

        /// <summary>
        /// 添加自定义的JWT处理程序 <see cref="TasselJwtHandler"/> 的扩展方法
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddTasselJwtBearer(this AuthenticationBuilder builder, Action<JwtBearerOptions> configureOptions)
            => builder.AddTasselJwtBearer(JwtBearerDefaults.AuthenticationScheme, configureOptions);

        /// <summary>
        /// 添加自定义的JWT处理程序 <see cref="TasselJwtHandler"/> 的扩展方法
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddTasselJwtBearer(this AuthenticationBuilder builder, string authenticationScheme, Action<JwtBearerOptions> configureOptions)
            => builder.AddTasselJwtBearer(authenticationScheme, displayName: null, configureOptions: configureOptions);

        /// <summary>
        /// 添加自定义的JWT处理程序 <see cref="TasselJwtHandler"/> 的扩展方法
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddTasselJwtBearer(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<JwtBearerOptions> configureOptions) {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>, JwtBearerPostConfigureOptions>());
            return builder.AddScheme<JwtBearerOptions, TasselJwtHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}
