using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LastMovie.Migrations
{
    public partial class change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Movie_ID",
                table: "InstallmentRecordTable",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Movie_ID",
                table: "InstallmentRecordTable");
        }
    }
}
