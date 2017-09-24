using Microsoft.AspNetCore.Mvc;
using Wallace.Core.Helpers.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tassel.Model.Models;

namespace Tassel.API.Utils.Extensions {
    public static class ControllersExtensions {

        public static JsonResult JsonFormat(
            this Controller c, 
            bool succeed, 
            JsonStatus status = JsonStatus.Succeed, 
            string error = "", 
            object content = null)
            => c.JsonFormat(new JsonBase {
               Status = status,
               Message = c.GetErrorMessage(succeed, error),
               Content = content
           });

    }
}
