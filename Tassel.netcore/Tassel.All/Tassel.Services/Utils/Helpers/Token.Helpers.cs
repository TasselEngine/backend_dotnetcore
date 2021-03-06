﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tassel.Services.Utils.Constants;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Tassel.Service.Utils.Helpers {

    public class TokenDecoder {

        public static TokenValidationParameters CreateParam(IConfiguration root) {
            return CreateParam(CreateKey(root));
        }

        public static TokenValidationParameters CreateParam(SecurityKey signingKey) {
            return new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = signingKey,
                ValidIssuer = TokenProviderEntry.Issuer,
                ValidAudience = TokenProviderEntry.Audience,
                ClockSkew = TimeSpan.Zero
            };
        }

        public static SecurityKey CreateKey(IConfigurationRoot root) {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(root.GetSection("TokenAccess")["Key"]));
        }

        public static SecurityKey CreateKey(IConfiguration root) {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(root.GetSection("TokenAccess")["Key"]));
        }

        public TokenDecoder(TokenValidationParameters param, string algorithm = SecurityAlgorithms.HmacSha256) {
            this.algorithm = algorithm;
            this.param = param;
        }

        public TokenDecoder(SecurityKey signingKey, string algorithm = SecurityAlgorithms.HmacSha256) {
            this.algorithm = algorithm;
            this.param = CreateParam(signingKey);
        }

        public TokenDecoder(IHostingEnvironment env) {
            this.algorithm = SecurityAlgorithms.HmacSha256;
            this.param = CreateParam(
                CreateKey(new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build()));
        }

        private readonly string algorithm;
        private readonly TokenValidationParameters param;

        public AuthenticationTicket Unprotect(string cookie, HttpContext context) {
            var handler = new JwtSecurityTokenHandler();
            var principal = default(ClaimsPrincipal);
            try {
                principal = handler.ValidateToken(cookie, this.param, out SecurityToken validToken);
                var validJwt = validToken as JwtSecurityToken;
                if (validJwt == null)
                    throw new ArgumentException("Invalid JWT");
                var uuid = validJwt.Claims.FirstOrDefault(i => i.Type == TokenClaimsKey.UUID);
                if (uuid != null) {
                    context.Items.Add(TokenClaimsKey.UUID, uuid.Value);
                }

                // TO DO : add more logic if need.

            } catch (SecurityTokenValidationException) {
                return null;
            } catch (ArgumentException) {
                return null;
            }
            return new AuthenticationTicket(principal, new AuthenticationProperties(), TokenProviderEntry.CookieScheme);
        }

        public (JwtSecurityToken, string) Decrypt(string cookie) {
            var handler = new JwtSecurityTokenHandler();
            var validJwt = default(JwtSecurityToken);
            try {
                _ = handler.ValidateToken(cookie, this.param, out SecurityToken validToken);
                validJwt = validToken as JwtSecurityToken;
                if (validJwt == null)
                    throw new ArgumentException("Invalid JWT");
                if (!validJwt.Header.Alg.Equals(algorithm, StringComparison.Ordinal))
                    throw new ArgumentException($"Algorithm must be '{algorithm}'");
            } catch (SecurityTokenValidationException e) {
                return (null, e.Message);
            } catch (ArgumentException e) {
                return (null, e.Message);
            }
            return (validJwt, null);
        }
    }

    public class TokenProviderOptions {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public TimeSpan Expiration { get; set; } = TimeSpan.FromDays(7);
        public string RegisterPath { get; set; } = TokenProviderEntry.RegisterPath;
        public string LoginPath { get; set; } = TokenProviderEntry.LoginPath;
        public string WeiboCheckPath { get; set; } = TokenProviderEntry.WeiboCheckPath;
        public SigningCredentials SigningCredentials { get; set; }
    }

}
