using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Models;
using Wallace.Core.Helpers.Providers;

namespace Tassel.Services.Contract {
    public interface IIdentityService<TToken, TOptions, TUser> {

        (TUser, bool, string) TryRegister(string user, string psd, Gender gender = Gender.Male, string avatar = null);

        (TUser, bool, string) TryLogin(string user, string psd);

        TToken GenerateToken(TUser user, TOptions options);

        TToken TokenDecrypt(string cookie);

        IEnumerable<dynamic> GetUsersListByFilter(Expression<Func<TUser, bool>> whereLambada);

        (TUser,bool, string) GetUserDetailsByID(string uuid);

    }
}
