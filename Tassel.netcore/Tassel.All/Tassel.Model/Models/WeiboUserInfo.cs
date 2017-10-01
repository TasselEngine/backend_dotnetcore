﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Wallace.Core.Helpers.Providers;

namespace Tassel.Model.Models {

    [Table("weibo_users")]
    public class WeiboDBUser{
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("uid")]
        public string UID { get; set; }

        [Column("access_token")]
        public string AccessToken { get; set; }

        [Column("screen_name")]
        public string ScreenName { get; set; }

        [Column("desc")]
        public string Description { get; set; }

        [Column("domain")]
        public string Domain { get; set; }

        [Column("avatar_url")]
        public string AvatarUrl { get; set; }

        [Column("cover_image_phone")]
        public string CoverMobile { get; set; }

        [Column("cover_image")]
        public string Cover { get; set; }

        [Column("c_time")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        [Column("u_time")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? UpdateTime { get; set; }
    }

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
