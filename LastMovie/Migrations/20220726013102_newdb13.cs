using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LastMovie.Migrations
{
    public partial class newdb13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstallmentLastPrice",
                table: "CartTable");

            migrationBuilder.AddColumn<bool>(
                name: "InstallmentLastBuy",
                table: "CartTable",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstallmentLastBuy",
                table: "CartTable");

            migrationBuilder.AddColumn<double>(
                name: "InstallmentLastPrice",
                table: "CartTable",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
