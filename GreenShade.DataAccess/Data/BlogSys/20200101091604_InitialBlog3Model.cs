using Microsoft.EntityFrameworkCore.Migrations;

namespace GreenShade.Blog.DataAccess.Data.BlogSys
{
    public partial class InitialBlog3Model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PicInfo",
                table: "Articles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PicUrl",
                table: "Articles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PicInfo",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "PicUrl",
                table: "Articles");
        }
    }
}
