using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Tassel.Model.Models {

    public enum GuidType { N, D, B, P }

    public enum Gender { Male, Female }

    [Table("users")]
    public class User {

        [Key]
        [Column("uuid")]
        public string UUID { get; set; }

        [Column("role_id")]
        public int RoleID { get; set; } = 3;

        [Column("u_name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username shouldn't be empty.")]
        public string UserName { get; set; }

        [Column("psd")]
        [IgnoreDataMember]
        public string Password { get; set; }

        [Column("email")]
        [EmailAddress(ErrorMessage = "Please input the correct email address.")]
        public string Email { get; set; }

        [Column("display_name")]
        public string DisplayName { get; set; }

        [Column("f_name")]
        public string FamilyName { get; set; }

        [Column("g_name")]
        public string GivenName { get; set; }

        [Column("gender")]
        [EnumDataType(enumType: typeof(Gender), ErrorMessage = "Please choose the correct gender.")]
        public Gender? Gender { get; set; }

        [Column("birth_date")]
        [DataType(DataType.Date, ErrorMessage = "Please choose the correct date.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; }

        [Column("c_time")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        [Column("u_time")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? UpdateTime { get; set; }

        [Column("is_3rd")]
        public bool IsThirdPart { get; set; } = false;

        [Column("weibo_id")]
        public string WeiboID { get; set; }

        [Column("wechat_token")]
        public string WechatToken { get; set; }

        [Column("qq_token")]
        public string QQToken { get; set; }

        [Column("avatar")]
        public string Avatar { get; set; }

    }

}
