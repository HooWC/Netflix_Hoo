using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LastMovie.Migrations
{
    public partial class newdb3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "TrTable");

            migrationBuilder.AddColumn<string>(
                name: "RoleID",
                table: "TrTable",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleID",
                table: "TrTable");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "TrTable",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
