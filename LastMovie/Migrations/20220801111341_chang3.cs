using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LastMovie.Migrations
{
    public partial class chang3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgList",
                table: "VideoTable",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgList",
                table: "VideoTable");
        }
    }
}
