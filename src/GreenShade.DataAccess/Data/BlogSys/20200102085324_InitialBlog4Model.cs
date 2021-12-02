using Microsoft.EntityFrameworkCore.Migrations;

namespace GreenShade.Blog.DataAccess.Data.BlogSys
{
    public partial class InitialBlog4Model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "Articles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Articles",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tag",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Articles");
        }
    }
}
