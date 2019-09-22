using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GreenShadow.Blog.DataAccess.Data.Blog
{
    public partial class InitialBlogModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    ArticleViews = table.Column<int>(nullable: false),
                    ArticleCommentCount = table.Column<int>(nullable: false),
                    ArticleDate = table.Column<DateTime>(nullable: false),
                    ArticleLikeCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");
        }
    }
}
