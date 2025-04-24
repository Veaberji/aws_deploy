using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusiciansAPP.DAL.DBDataProvider.Migrations
{
    public partial class AddDescriptionToAlbum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Albums",
                type: "ntext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Albums");
        }
    }
}
