using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LastMovie.Migrations
{
    public partial class newdb12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "InstallmentLastPrice",
                table: "CartTable",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "InstallmentLastTime",
                table: "CartTable",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstallmentLastPrice",
                table: "CartTable");

            migrationBuilder.DropColumn(
                name: "InstallmentLastTime",
                table: "CartTable");
        }
    }
}
