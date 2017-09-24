using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Linq;

namespace Wallace.Core.Helpers.Controllers {

    public static class ControllersExtensions {

        public static JsonResult JsonFormat(this Controller c, object model)
            => c.Json(model,
                new JsonSerializerSettings {
                    ContractResolver = new LowercaseContractResolver(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented
                });

        public static IEnumerable<string> GetModelErrors(this Controller c)
            => c.ModelState.Values.Select(i => i.Errors.Select(s => s.ErrorMessage).FirstOrDefault());

        public static string GetErrorMessage(this Controller c, bool succeed = true, string msg = "")
            => succeed ? "success" : msg;
    }

    public class LowercaseContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver {
        protected override string ResolvePropertyName(string propertyName) {
            var coll = new Regex(@"[a-z]{1}[A-Z]{1}|[A-Z]{2}[a-z]{1}").Matches(propertyName);
            foreach (Match item in coll) {
                propertyName = propertyName.Replace(item.Value, item.Value.Substring(0, 1) + "_" + item.Value.Substring(1));
            }
            return propertyName.ToLower();
        }
    }

}
