using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusiciansAPP.DAL.DBDataProvider.Migrations
{
    public partial class AddSimilarArtistsToArtist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SimilarArtists",
                columns: table => new
                {
                    ReverseSimilarArtistsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SimilarArtistsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimilarArtists", x => new { x.ReverseSimilarArtistsId, x.SimilarArtistsId });
                    table.ForeignKey(
                        name: "FK_SimilarArtists_Artists_ReverseSimilarArtistsId",
                        column: x => x.ReverseSimilarArtistsId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SimilarArtists_Artists_SimilarArtistsId",
                        column: x => x.SimilarArtistsId,
                        principalTable: "Artists",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SimilarArtists_SimilarArtistsId",
                table: "SimilarArtists",
                column: "SimilarArtistsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SimilarArtists");
        }
    }
}
