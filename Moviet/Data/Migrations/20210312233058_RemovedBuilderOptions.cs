using Microsoft.EntityFrameworkCore.Migrations;

namespace Moviet.Data.Migrations
{
    public partial class RemovedBuilderOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Movies_MovieId1",
                table: "MovieGenres");

            migrationBuilder.DropIndex(
                name: "IX_MovieGenres_MovieId1",
                table: "MovieGenres");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "MovieGenres");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MovieId1",
                table: "MovieGenres",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenres_MovieId1",
                table: "MovieGenres",
                column: "MovieId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Movies_MovieId1",
                table: "MovieGenres",
                column: "MovieId1",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
