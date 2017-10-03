using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tassel.API.Utils.Helpers {
    public static class TokenProvider {

        public static SecurityKey CreateKey(IConfigurationRoot root) {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(root.GetSection("TokenAccess")["Key"]));
        }

        public static SecurityKey CreateKey(IConfiguration root) {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(root.GetSection("TokenAccess")["Key"]));
        }

    }
}
