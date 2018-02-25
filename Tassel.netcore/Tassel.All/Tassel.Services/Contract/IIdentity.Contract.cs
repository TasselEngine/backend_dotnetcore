using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tassel.Model.Models.BsonModels;
using Tassel.Services.Contract.Providers;

namespace Tassel.Services.Contract {
    public interface IIdentityService<TToken, TOptions, TUser> {

        IWeiboOAuthServiceProvider<TUser> WeiboService { get; }

        (TUser user, bool succeed, string error) TryRegister(string user, string psd, Gender gender = Gender.Male, string avatar = null);

        (TUser user, bool succeed, string error) TryLogin(string user, string psd);

        TToken GenerateToken(TUser user, TOptions options);

        TToken TokenDecrypt(string cookie);

        IEnumerable<dynamic> GetUsersListByFilter(Expression<Func<TUser, bool>> whereLambada);

        (TUser user, bool succeed, string error) GetUserDetailsByID(string uuid);

        (TUser user, bool succeed, string error) GetUserDetailsByUserName(string uname);
        
        (bool succeed, string error) TryUpdateNative(TUser user);

        (string role, bool succeed, string error) GetUserRole(string uuid);

        (bool succeed, string error) UpdateUserRole(string uid, string newRole);

    }
}
