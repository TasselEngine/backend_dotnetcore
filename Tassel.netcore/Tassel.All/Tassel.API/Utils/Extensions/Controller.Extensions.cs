﻿using Microsoft.AspNetCore.Mvc;
using BWS.Utils.AspNetCore.Controllers;
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
            string error = default(string), 
            dynamic content = default(dynamic))
            => c.JsonFormat(new JsonBase {
               Status = status,
               Message = c.GetErrorMessage(succeed, error??JsonErrorMaps.TryGet(status)),
               Content = content
           });

    }
}