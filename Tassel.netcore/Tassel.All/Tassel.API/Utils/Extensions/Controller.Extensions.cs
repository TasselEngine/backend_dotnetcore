using Microsoft.AspNetCore.Mvc;
using BWS.Utils.AspNetCore.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using Tassel.Services.Utils.Constants;

namespace Tassel.API.Utils.Extensions {
    public static class ControllersExtensions {

        public static JsonResult JsonFormat(
            this Controller c,
            bool succeed,
            JsonStatus status = JsonStatus.Succeed,
            string error = default(string),
            dynamic content = default(dynamic))
            => c.JsonFormat(succeed ? new JsonBase {
                Status = status,
                Message = c.GetErrorMessage(succeed, error ?? JsonErrorMaps.TryGet(status)),
                Content = content
            } : new JsonBase {
                Status = status == JsonStatus.Succeed ? JsonStatus.Error : status,
                Message = c.GetErrorMessage(succeed, error ?? JsonErrorMaps.TryGet(status == JsonStatus.Succeed ? JsonStatus.Error : status)),
                Content = content
            });

        public static BaseCreator GetUser(this Controller controller) {
            controller.HttpContext.GetStringEntry(TokenClaimsKey.UUID, out var uuid);
            controller.HttpContext.GetStringEntry(TokenClaimsKey.Avatar, out var avatar);
            controller.HttpContext.GetStringEntry(TokenClaimsKey.UserName, out var uname);
            return new BaseCreator { UUID = uuid, UserName = uname, AvatarUrl = avatar };
        }

        public static string GetRole(this Controller c) {
            c.HttpContext.GetStringEntry(TokenClaimsKey.RoleID, out var role);
            return role;
        }

    }
}
