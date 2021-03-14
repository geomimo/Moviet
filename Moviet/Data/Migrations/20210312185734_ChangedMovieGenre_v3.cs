using Microsoft.EntityFrameworkCore.Migrations;

namespace Moviet.Data.Migrations
{
    public partial class ChangedMovieGenre_v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Movies_GenreId",
                table: "MovieGenres");

            migrationBuilder.AddColumn<int>(
                name: "MovieId1",
                table: "MovieGenres",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenres_MovieId1",
                table: "MovieGenres",
                column: "MovieId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Movies_MovieId",
                table: "MovieGenres",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Movies_MovieId1",
                table: "MovieGenres",
                column: "MovieId1",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Movies_MovieId",
                table: "MovieGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Movies_MovieId1",
                table: "MovieGenres");

            migrationBuilder.DropIndex(
                name: "IX_MovieGenres_MovieId1",
                table: "MovieGenres");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "MovieGenres");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Movies_GenreId",
                table: "MovieGenres",
                column: "GenreId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
