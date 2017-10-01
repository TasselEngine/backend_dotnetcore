using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tassel.API.Utils.Extensions {
    public static class HttpContextExtensions {

        public static void SetStringEntry(this HttpContext context,string key, string value) {
            context.Items.Add(key, value);
        }

        public static void GetStringEntry(this HttpContext context, string key, out string value) {
            context.Items.TryGetValue(key, out var v);
            value = v as string;
        }

    }
}
