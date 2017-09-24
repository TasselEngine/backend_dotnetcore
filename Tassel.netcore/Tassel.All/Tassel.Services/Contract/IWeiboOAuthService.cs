using System;
using System.Collections.Generic;
using System.Text;
using Tassel.Model.Models;
using Wallace.Core.Helpers.Providers;

namespace Tassel.Services.Contract {
    public interface IWeiboOAuthService<TUser> : IWeiboOAuthV2Provider<WeiboSuccessToken, WeiboUser> {

        (TUser, bool, string) TryCreateOrGetUserByWeibo(WeiboUser wuser);

        (TUser, bool, string) TryGetUserByWeibo(string uid);

        (WeiboDBUser, bool, string) SearchWeiboUserInfoByUID(string uid);

    }
}
