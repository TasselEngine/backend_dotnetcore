using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Tassel.Model.Models;
using WeiboOAuth2.Provider.Src;

namespace Tassel.Model.Utils {

    public static class IdentityProvider {

        public static User CreateUser(string username, string password, Gender gender, string avatar) {
            using (var md5 = MD5.Create()) {
                return new User {
                    UUID = CreateGuid(GuidType.N),
                    UserName = username,
                    Password = CreateMD5(password),
                    Gender = gender,
                    Avatar = avatar,
                    RoleID = 3,
                };
            }
        }

        public static User CreateUserByWeibo(WeiboUser wuser) {
            var uuid = CreateGuid(GuidType.N);
            return new User {
                UUID = uuid,
                UserName = "WUSER_" + uuid,
                Gender = wuser.gender == "m" ? Gender.Male : Gender.Female,
                WeiboID = wuser.idstr,
                RoleID = 3,
                IsThirdPart = true
            };
        }

        public static User CreateUser(string username, string password)
            => CreateUser(username, password, Gender.Male, null);

        public static string CreateMD5(string input) {
            if (input == null)
                return null;
            using (var md5 = MD5.Create()) {
                return BitConverter
                    .ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(input)))
                    .Replace("-", "")
                    .ToLower();
            }
        }

        public static string CreateGuid(GuidType type = GuidType.D)
            => Guid.NewGuid().ToString(
                type == GuidType.B ? "B" :
                type == GuidType.D ? "D" :
                type == GuidType.N ? "N" :
                "P");

    }

}
