using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LastMovie.Migrations
{
    public partial class newdb4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "CartTable");

            migrationBuilder.AddColumn<string>(
                name: "RoleID",
                table: "CartTable",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleID",
                table: "CartTable");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "CartTable",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
