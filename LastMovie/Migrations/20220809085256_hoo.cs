using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LastMovie.Migrations
{
    public partial class hoo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConnID",
                table: "CommentTable",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnID",
                table: "CommentTable");
        }
    }
}
