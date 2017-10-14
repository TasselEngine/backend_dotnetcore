using System;
using Tassel.Model.Utils;
using Xunit;

namespace Tassel.TEST {

    public class WeiboUserUtils_TEST {
        [Fact]
        public void TEST_CreateWeiboUser() {

            var wuser = WeiboUserProvider.CreateUser(new WeiboOAuth2.Provider.WeiboUser {
                id = 123,
                idstr = "123",
                screen_name = "test_user",
                description = "test_desc"
            }, "test_token");

            Assert.NotNull(wuser);
            Assert.Equal("test_desc", wuser.Description);
            Assert.Equal("123", wuser.UID);
            Assert.Equal("test_user", wuser.ScreenName);
            Assert.Equal("test_token", wuser.AccessToken);

            var wuser2 = WeiboUserProvider.CreateUser( null , "test_token");

            Assert.Null(wuser2);

        }
    }

}
