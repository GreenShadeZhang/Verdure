using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GreenShade.Blog.DataAccess.Data.BlogSys
{
    public partial class InitialBlogSys5Model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WnsUrls",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DevFamily = table.Column<string>(nullable: true),
                    PushUrl = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WnsUrls", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WnsUrls");
        }
    }
}
