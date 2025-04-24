using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusiciansAPP.DAL.DBDataProvider.Migrations
{
    public partial class IncreaseArtistBiographySize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Biography",
                table: "Artists",
                type: "ntext",
                maxLength: 10000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(3500)",
                oldMaxLength: 3500,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Biography",
                table: "Artists",
                type: "nvarchar(3500)",
                maxLength: 3500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldMaxLength: 10000,
                oldNullable: true);
        }
    }
}
