using System;
using System.Collections.Generic;
using System.Text;
using Tassel.Model.Models;
using Tassel.Model.Models.BsonModels;
using WeiboOAuth2.Provider;

namespace Tassel.Model.Utils {

    public static class WeiboUserProvider {

        public static WeiboDBUser CreateUser(WeiboUser wuser, string access_token) {
            return new WeiboDBUser {
                UID = wuser.idstr,
                ScreenName = wuser.screen_name,
                Description = wuser.description,
                Domain = wuser.domain,
                AvatarUrl = wuser.avatar_large,
                Cover = wuser.cover_image,
                CoverMobile = wuser.cover_image_phone,
                AccessToken = access_token
            };
        }

        public static WeiboDBUser Update(this WeiboDBUser wuser, WeiboUser newUser, string access_token = null) {
            wuser.AvatarUrl = newUser.avatar_large;
            wuser.Cover = newUser.cover_image;
            wuser.CoverMobile = newUser.cover_image_phone;
            wuser.Description = newUser.description;
            wuser.Domain = newUser.domain;
            wuser.ScreenName = newUser.screen_name;
            wuser.UpdateTime = DateTime.UtcNow;
            if (!string.IsNullOrEmpty(access_token)) {
                wuser.AccessToken = access_token;
            }
            return wuser;
        }

    }

}
