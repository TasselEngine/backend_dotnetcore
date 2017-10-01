using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Wallace.Core.Helpers.Providers {

    public interface IWeiboOAuthV2Option {
        string AppID { get; }
        string AppSecret { get; }
    }

    public interface IWeiboOAuthV2Provider<TToken, TWeiboUser> {
        Task<(TToken, bool, string)> GetWeiboTokenByCodeAsync(string code, string redirect_url);
        Task<(TWeiboUser, bool, string)> GetWeiboUserInfosAsync(string uid, string access_token);
        Task<(RevokeOAuth2Return, bool, string)> RevokeOAuth2Access(string access_token);
    }

    public class WeiboOAuthV2Provider : IWeiboOAuthV2Provider<WeiboSuccessToken, WeiboUser> {

        protected IWeiboOAuthV2Option options;

        public WeiboOAuthV2Provider(IWeiboOAuthV2Option options) {
            this.options = options;
        }

        public async Task<(WeiboSuccessToken, bool, string)> GetWeiboTokenByCodeAsync(string code, string redirect_url) {
            var oars = $"client_id={options.AppID}&client_secret={options.AppSecret}&grant_type=authorization_code";
            HttpContent hc = new StringContent(oars, Encoding.UTF8, "application/x-www-form-urlencoded");
            using (var client = new HttpClient()) {
                try {
                    var result = await client.PostAsync($"https://api.weibo.com/oauth2/access_token?code={code}&redirect_uri={redirect_url}", hc);
                    var conStr = await result.Content.ReadAsStringAsync();
                    try {
                        var succ_token = JsonConvert.DeserializeObject<WeiboSuccessToken>(conStr);
                        var fail_token = default(WeiboErrorToken);
                        try {
                            fail_token = JsonConvert.DeserializeObject<WeiboErrorToken>(conStr);
                        } catch {
                            return (default(WeiboSuccessToken), false, WeiboOAuth2Errors.GetErrorMessageFailed);
                        }
                        return succ_token == null || succ_token.AccessToken == null ?
                            (default(WeiboSuccessToken), false, fail_token?.ErrorDescription) :
                            (succ_token, true, null);
                    } catch {
                        return (default(WeiboSuccessToken), false, WeiboOAuth2Errors.DeserializeAccessTokenFailed);
                    }
                } catch {
                    return (default(WeiboSuccessToken), false, WeiboOAuth2Errors.GetAccessTokenFailed);
                }
            }
        }

        public async Task<(WeiboUser, bool, string)> GetWeiboUserInfosAsync(string uid, string access_token) {
            using (var client = new HttpClient()) {
                try {
                    var result = await client.GetAsync($"https://api.weibo.com/2/users/show.json?access_token={access_token}&uid={uid}");
                    var conStr = await result.Content.ReadAsStringAsync();
                    try {
                        return (JsonConvert.DeserializeObject<WeiboUser>(conStr), true, conStr);
                    } catch {
                        return (default(WeiboUser), false, WeiboOAuth2Errors.DeserializeWeiboUserInfosFailed);
                    }
                } catch {
                    return (default(WeiboUser), false, WeiboOAuth2Errors.GetWeiboUserInfosFailed);
                }
            }
        }

        public async Task<(RevokeOAuth2Return, bool, string)> RevokeOAuth2Access(string access_token) {
            using (var client = new HttpClient()) {
                try {
                    var result = await client.GetAsync($"https://api.weibo.com/oauth2/revokeoauth2?access_token={access_token}");
                    var conStr = await result.Content.ReadAsStringAsync();
                    try {
                        return (JsonConvert.DeserializeObject<RevokeOAuth2Return>(conStr), true, null);
                    } catch {
                        return (default(RevokeOAuth2Return), false, WeiboOAuth2Errors.DeserializeRevokeTokenFailed);
                    }
                } catch {
                    return (default(RevokeOAuth2Return), false, WeiboOAuth2Errors.GetRevokeTokenFailed);
                }
            }
        }
    }

    [JsonObject]
    public class WeiboErrorToken {

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("error_code")]
        public int ErrorCode { get; set; }

        [JsonProperty("request")]
        public string Request { get; set; }

        [JsonProperty("error_uri")]
        public string ErrorUri { get; set; }

        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }

    }

    [JsonObject]
    public class WeiboSuccessToken {

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("remind_in")]
        public string RemindIn { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("isRealName")]
        public bool IsRealName { get; set; }

    }

    [JsonObject]
    public class RevokeOAuth2Return {

        [JsonProperty("result")]
        private string ReturnStr = "false";
        public bool ShouldDeserializeReturnStr() { return true; }
        public bool ShouldSerializeReturnStr() { return false; }

        [JsonProperty("return")]
        public bool Return { get => ReturnStr == "true"; }
        public bool ShouldDeserializeReturn() { return false; }
        public bool ShouldSerializeReturn() { return true; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("error_code")]
        public int ErrorCode { get; set; }

        [JsonProperty("request")]
        public string Request { get; set; }

        [JsonProperty("error_uri")]
        public string ErrorUri { get; set; }

        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }

    }

    public static class WeiboOAuth2Errors {
        public const string GetErrorMessageFailed = "try get error message from response failed.";
        public const string GetAccessTokenFailed = "get weibo access token failed.";
        public const string GetWeiboUserInfosFailed = "get weibo user infos failed.";
        public const string GetRevokeTokenFailed = "revoke failed. unknown error.";
        public const string DeserializeAccessTokenFailed = "try read weibo access token from response failed.";
        public const string DeserializeWeiboUserInfosFailed = "try read weibo userinfos from response failed.";
        public const string DeserializeRevokeTokenFailed = "try read weibo revoke message from response failed.";
    }

    public class WeiboUser {
        public long id { get; set; }
        public string idstr { get; set; }
        public int _class { get; set; }
        public string screen_name { get; set; }
        public string name { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string location { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string profile_image_url { get; set; }
        public string cover_image { get; set; }
        public string cover_image_phone { get; set; }
        public string profile_url { get; set; }
        public string domain { get; set; }
        public string weihao { get; set; }
        public string gender { get; set; }
        public int followers_count { get; set; }
        public int friends_count { get; set; }
        public int pagefriends_count { get; set; }
        public int statuses_count { get; set; }
        public int favourites_count { get; set; }
        public string created_at { get; set; }
        public bool following { get; set; }
        public bool allow_all_act_msg { get; set; }
        public bool geo_enabled { get; set; }
        public bool verified { get; set; }
        public int verified_type { get; set; }
        public string remark { get; set; }
        public Insecurity insecurity { get; set; }
        public Status status { get; set; }
        public int ptype { get; set; }
        public bool allow_all_comment { get; set; }
        public string avatar_large { get; set; }
        public string avatar_hd { get; set; }
        public string verified_reason { get; set; }
        public string verified_trade { get; set; }
        public string verified_reason_url { get; set; }
        public string verified_source { get; set; }
        public string verified_source_url { get; set; }
        public bool follow_me { get; set; }
        public bool like { get; set; }
        public bool like_me { get; set; }
        public int online_status { get; set; }
        public int bi_followers_count { get; set; }
        public string lang { get; set; }
        public int star { get; set; }
        public int mbtype { get; set; }
        public int mbrank { get; set; }
        public int block_word { get; set; }
        public int block_app { get; set; }
        public int credit_score { get; set; }
        public int user_ability { get; set; }
        public string cardid { get; set; }
        public string avatargj_id { get; set; }
        public int urank { get; set; }
        public int story_read_state { get; set; }
        public int vclub_member { get; set; }
    }

    public class Insecurity {
        public bool sexual_content { get; set; }
    }

    public class Status {
        public string created_at { get; set; }
        public long id { get; set; }
        public string mid { get; set; }
        public string idstr { get; set; }
        public string text { get; set; }
        public int textLength { get; set; }
        public int source_allowclick { get; set; }
        public int source_type { get; set; }
        public string source { get; set; }
        public bool favorited { get; set; }
        public bool truncated { get; set; }
        public string in_reply_to_status_id { get; set; }
        public string in_reply_to_user_id { get; set; }
        public string in_reply_to_screen_name { get; set; }
        public PicUrls[] pic_urls { get; set; }
        public string thumbnail_pic { get; set; }
        public string bmiddle_pic { get; set; }
        public string original_pic { get; set; }
        public object geo { get; set; }
        public bool is_paid { get; set; }
        public int mblog_vip_type { get; set; }
        public Annotation[] annotations { get; set; }
        public string stickerID { get; set; }
        public int reposts_count { get; set; }
        public int comments_count { get; set; }
        public int attitudes_count { get; set; }
        public bool isLongText { get; set; }
        public int mlevel { get; set; }
        public Visible visible { get; set; }
        public long biz_feature { get; set; }
        public int page_type { get; set; }
        public int hasActionTypeCard { get; set; }
        public object[] darwin_tags { get; set; }
        public object[] hot_weibo_tags { get; set; }
        public object[] text_tag_tips { get; set; }
        public int userType { get; set; }
        public int more_info_type { get; set; }
        public string cardid { get; set; }
        public int positive_recom_flag { get; set; }
        public string gif_ids { get; set; }
        public int is_show_bulletin { get; set; }
        public CommentManageInfo comment_manage_info { get; set; }
    }

    public class Visible {
        public int type { get; set; }
        public int list_id { get; set; }
    }

    public class CommentManageInfo {
        public int comment_permission_type { get; set; }
    }

    public class PicUrls {
        public string thumbnail_pic { get; set; }
    }

    public class Annotation {
        public string client_mblogid { get; set; }
        public bool mapi_request { get; set; }
    }

}
