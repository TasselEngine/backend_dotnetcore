﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using Tassel.Model.Models;

namespace Tassel.Model.Migrations
{
    [DbContext(typeof(APIDB))]
    partial class APIDBModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Tassel.Model.Models.User", b =>
                {
                    b.Property<string>("UUID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("uuid");

                    b.Property<string>("Avatar")
                        .HasColumnName("avatar");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnName("birth_date");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnName("c_time");

                    b.Property<string>("Email")
                        .HasColumnName("email");

                    b.Property<string>("FamilyName")
                        .HasColumnName("f_name");

                    b.Property<int?>("Gender")
                        .HasColumnName("gender");

                    b.Property<string>("GivenName")
                        .HasColumnName("g_name");

                    b.Property<bool>("IsThirdPart")
                        .HasColumnName("is_3rd");

                    b.Property<string>("Password")
                        .HasColumnName("psd");

                    b.Property<string>("QQToken")
                        .HasColumnName("qq_token");

                    b.Property<int>("RoleID")
                        .HasColumnName("role_id");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnName("u_time");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnName("u_name");

                    b.Property<string>("WechatToken")
                        .HasColumnName("wechat_token");

                    b.Property<string>("WeiboID")
                        .HasColumnName("weibo_id");

                    b.HasKey("UUID");

                    b.ToTable("users");
                });

            modelBuilder.Entity("Tassel.Model.Models.WeiboDBUser", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("AvatarUrl")
                        .HasColumnName("avatar_url");

                    b.Property<string>("Cover")
                        .HasColumnName("cover_image");

                    b.Property<string>("CoverMobile")
                        .HasColumnName("cover_image_phone");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnName("c_time");

                    b.Property<string>("Description")
                        .HasColumnName("desc");

                    b.Property<string>("Domain")
                        .HasColumnName("domain");

                    b.Property<string>("ScreenName")
                        .HasColumnName("screen_name");

                    b.Property<string>("UID")
                        .HasColumnName("uid");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnName("u_time");

                    b.HasKey("ID");

                    b.ToTable("weibo_users");
                });
#pragma warning restore 612, 618
        }
    }
}
