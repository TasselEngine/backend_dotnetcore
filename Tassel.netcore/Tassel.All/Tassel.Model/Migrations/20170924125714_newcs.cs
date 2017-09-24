using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Tassel.Model.Migrations
{
    public partial class newcs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "cover_image",
                table: "weibo_users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cover_image_phone",
                table: "weibo_users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cover_image",
                table: "weibo_users");

            migrationBuilder.DropColumn(
                name: "cover_image_phone",
                table: "weibo_users");
        }
    }
}
