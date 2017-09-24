using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Tassel.Model.Models;
using Newtonsoft.Json;
using Wallace.Core.Helpers.Controllers;
using System.IdentityModel.Tokens.Jwt;
using Tassel.Service.Utils.Extensionss;

namespace Tassel.API.Utils.Handlers {
    /// <summary>
    /// 自定义的JWT处理程序
    /// </summary>
    public class TasselJwtHandler : AuthenticationHandler<JwtBearerOptions> {
        private OpenIdConnectConfiguration _configuration;

        public TasselJwtHandler(IOptionsMonitor<JwtBearerOptions> options, ILoggerFactory logger, UrlEncoder encoder, IDataProtectionProvider dataProtection, ISystemClock clock)
            : base(options, logger, encoder, clock) { }

        /// <summary>
        /// 处理程序调用事件的方法, 在发生处理的特定点上给应用程序控制。
        /// 如果未提供默认实例, 则在调用方法时不执行任何设置。
        /// </summary>
        protected new JwtBearerEvents Events {
            get { return (JwtBearerEvents)base.Events; }
            set { base.Events = value; }
        }

        protected override Task<object> CreateEventsAsync() => Task.FromResult<object>(new JwtBearerEvents());

        /// <summary>
        /// 在 "授权" 标头中搜索 "持票人" 令牌。如果找到 "持票人" 令牌, 则使用 <see cref="TokenValidationParameters"/> 在选项设置.
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
            string token = null;
            try {
                // 提供从不同位置查找、调整或拒绝令牌的应用程序机会
                var messageReceivedContext = new MessageReceivedContext(Context, Scheme, Options);
                // 事件可以设置令牌
                await Events.MessageReceived(messageReceivedContext);
                if (messageReceivedContext.Result != null) {
                    return messageReceivedContext.Result;
                }
                // 如果应用程序从别处检索到标记, 请使用.
                token = messageReceivedContext.Token;

                if (string.IsNullOrEmpty(token)) {
                    string authorization = Request.Headers["Authorization"];
                    // 如果未找到授权标头, 则无法进一步处理
                    if (string.IsNullOrEmpty(authorization)) {
                        return AuthenticateResult.NoResult();
                    }
                    if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)) {
                        token = authorization.Substring("Bearer ".Length).Trim();
                    }
                    // 如果找不到任何令牌, 则无法进一步工作
                    if (string.IsNullOrEmpty(token)) {
                        return AuthenticateResult.NoResult();
                    }
                }

                if (_configuration == null && Options.ConfigurationManager != null) {
                    _configuration = await Options.ConfigurationManager.GetConfigurationAsync(Context.RequestAborted);
                }

                var validationParameters = Options.TokenValidationParameters.Clone();
                if (_configuration != null) {
                    var issuers = new[] { _configuration.Issuer };
                    validationParameters.ValidIssuers = validationParameters.ValidIssuers?.Concat(issuers) ?? issuers;
                    validationParameters.IssuerSigningKeys = validationParameters.IssuerSigningKeys?.Concat(_configuration.SigningKeys)
                        ?? _configuration.SigningKeys;
                }

                List<Exception> validationFailures = null;
                SecurityToken validatedToken;
                foreach (var validator in Options.SecurityTokenValidators) {
                    if (validator.CanReadToken(token)) {
                        ClaimsPrincipal principal;
                        try {
                            principal = validator.ValidateToken(token, validationParameters, out validatedToken);

                            var validJwt = validatedToken as JwtSecurityToken;
                            
                            if (validJwt == null)
                                throw new ArgumentException("Invalid JWT");
                            var uuid = validJwt.Claims.FirstOrDefault(i => i.Type == TokenClaimsKey.UUID);
                            if (uuid != null)
                                this.Context.Items.Add(TokenClaimsKey.UUID, uuid.Value);

                        } catch (ArgumentException ex) {
                            if (validationFailures == null) {
                                validationFailures = new List<Exception>(1);
                            }
                            validationFailures.Add(ex);
                            continue;
                        } catch (Exception ex) {
                            //Logger.TokenValidationFailed(token, ex);
                            //为可能由键翻转引起的异常刷新配置。用户还可以请求在事件中进行刷新。
                            if (Options.RefreshOnIssuerKeyNotFound && Options.ConfigurationManager != null
                                && ex is SecurityTokenSignatureKeyNotFoundException) {
                                Options.ConfigurationManager.RequestRefresh();
                            }
                            if (validationFailures == null) {
                                validationFailures = new List<Exception>(1);
                            }
                            validationFailures.Add(ex);
                            continue;
                        }

                        //Logger.TokenValidationSucceeded();
                        var tokenValidatedContext = new TokenValidatedContext(Context, Scheme, Options) {
                            Principal = principal,
                            SecurityToken = validatedToken
                        };
                        await Events.TokenValidated(tokenValidatedContext);
                        if (tokenValidatedContext.Result != null) {
                            return tokenValidatedContext.Result;
                        }
                        if (Options.SaveToken) {
                            tokenValidatedContext.Properties.StoreTokens(new[] {
                                new AuthenticationToken { Name = "access_token", Value = token }
                            });
                        }
                        tokenValidatedContext.Success();
                        return tokenValidatedContext.Result;
                    }
                }

                if (validationFailures != null) {
                    var authenticationFailedContext = new AuthenticationFailedContext(Context, Scheme, Options) {
                        Exception = (validationFailures.Count == 1) ? validationFailures[0] : new AggregateException(validationFailures)
                    };
                    await Events.AuthenticationFailed(authenticationFailedContext);
                    if (authenticationFailedContext.Result != null) {
                        return authenticationFailedContext.Result;
                    }
                    return AuthenticateResult.Fail(authenticationFailedContext.Exception);
                }
                return AuthenticateResult.Fail("No SecurityTokenValidator available for token: " + token ?? "[null]");
            } catch (Exception ex) {
                //Logger.ErrorProcessingMessage(ex);
                var authenticationFailedContext = new AuthenticationFailedContext(Context, Scheme, Options) {
                    Exception = ex
                };
                await Events.AuthenticationFailed(authenticationFailedContext);
                if (authenticationFailedContext.Result != null) {
                    return authenticationFailedContext.Result;
                }
                throw;
            }
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties) {
            var authResult = await HandleAuthenticateOnceSafeAsync();
            var eventContext = new JwtBearerChallengeContext(Context, Scheme, Options, properties) {
                AuthenticateFailure = authResult?.Failure
            };
            // 避免返回错误 = invalid_token 如果错误不是由身份验证失败引起的 (例如, 缺少令牌)。
            if (Options.IncludeErrorDetails && eventContext.AuthenticateFailure != null) {
                eventContext.Error = "invalid_token";
                eventContext.ErrorDescription = CreateErrorDescription(eventContext.AuthenticateFailure);
            }
            await Events.Challenge(eventContext);
            if (eventContext.Handled) {
                return;
            }
            //  无论如何保证请求成功，用status表示具体api结果
            Response.StatusCode = 200;
            if (string.IsNullOrEmpty(eventContext.Error) &&
                string.IsNullOrEmpty(eventContext.ErrorDescription) &&
                string.IsNullOrEmpty(eventContext.ErrorUri)) {
                Response.Headers.Append(HeaderNames.WWWAuthenticate, Options.Challenge);
            } else {
                // https://tools.ietf.org/html/rfc6750#section-3.1
                //WWW 认证: 无记名领域 = "示例", 错误 = "invalid_token", error_description = "访问令牌过期"
                var builder = new StringBuilder(Options.Challenge);
                if (Options.Challenge.IndexOf(" ", StringComparison.Ordinal) > 0) {
                    // 仅在第一个参数之后添加逗号 (如果有)
                    builder.Append(',');
                }
                if (!string.IsNullOrEmpty(eventContext.Error)) {
                    builder.Append(" error=\"");
                    builder.Append(eventContext.Error);
                    builder.Append("\"");
                }
                if (!string.IsNullOrEmpty(eventContext.ErrorDescription)) {
                    if (!string.IsNullOrEmpty(eventContext.Error)) {
                        builder.Append(",");
                    }
                    builder.Append(" error_description=\"");
                    builder.Append(eventContext.ErrorDescription);
                    builder.Append('\"');
                }
                if (!string.IsNullOrEmpty(eventContext.ErrorUri)) {
                    if (!string.IsNullOrEmpty(eventContext.Error) ||
                        !string.IsNullOrEmpty(eventContext.ErrorDescription)) {
                        builder.Append(",");
                    }
                    builder.Append(" error_uri=\"");
                    builder.Append(eventContext.ErrorUri);
                    builder.Append('\"');
                }
                Response.Headers.Append(HeaderNames.WWWAuthenticate, builder.ToString());
            }

            await Response.WriteAsync(JsonConvert.SerializeObject(new JsonBase {
                Status = JsonStatus.Error,
                Message = "Bearer token check failed."
            }, new JsonSerializerSettings {
                ContractResolver = new LowercaseContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            }));
        }

        private static string CreateErrorDescription(Exception authFailure) {
            IEnumerable<Exception> exceptions;
            if (authFailure is AggregateException) {
                var agEx = authFailure as AggregateException;
                exceptions = agEx.InnerExceptions;
            } else {
                exceptions = new[] { authFailure };
            }

            var messages = new List<string>();

            foreach (var ex in exceptions) {
                // 顺序敏感, 其中一些例外从其他派生
                // 我们希望显示最具体的信息可能。
                if (ex is SecurityTokenInvalidAudienceException) {
                    messages.Add("The audience is invalid");
                } else if (ex is SecurityTokenInvalidIssuerException) {
                    messages.Add("The issuer is invalid");
                } else if (ex is SecurityTokenNoExpirationException) {
                    messages.Add("The token has no expiration");
                } else if (ex is SecurityTokenInvalidLifetimeException) {
                    messages.Add("The token lifetime is invalid");
                } else if (ex is SecurityTokenNotYetValidException) {
                    messages.Add("The token is not valid yet");
                } else if (ex is SecurityTokenExpiredException) {
                    messages.Add("The token is expired");
                } else if (ex is SecurityTokenSignatureKeyNotFoundException) {
                    messages.Add("The signature key was not found");
                } else if (ex is SecurityTokenInvalidSignatureException) {
                    messages.Add("The signature is invalid");
                }
            }

            return string.Join("; ", messages);
        }
    }
}
