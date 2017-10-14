using System;
using System.Collections.Generic;
using System.Text;
using Tassel.Model.Models.BsonModels;
using WeiboOAuth2.Provider;

namespace Tassel.Services.Contract.Providers {
    public interface IWeiboOAuthServiceProvider<TUser> : IWeiboOAuthV2Provider<WeiboSuccessToken, WeiboUser> {

        (TUser, bool, string) TryCreateOrUpdateUserByWeibo(WeiboUser wuser, string access_token);

        (TUser, bool, string) TryGetUserByWeibo(string uid);

        (WeiboDBUser, bool, string) SearchWeiboUserInfoByUID(string uid);

    }
}
