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

        public static WeiboDBUser CreateUser(WeiboUser wuser) {
            return new WeiboDBUser {
                UID = wuser.idstr,
                ScreenName = wuser.screen_name,
                Description = wuser.description,
                Domain = wuser.domain,
                AvatarUrl = wuser.avatar_large,
                Cover = wuser.cover_image,
                CoverMobile = wuser.cover_image_phone
            };
        }

    }

}
