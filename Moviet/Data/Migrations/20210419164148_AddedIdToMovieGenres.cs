using Microsoft.EntityFrameworkCore.Migrations;

namespace Moviet.Data.Migrations
{
    public partial class AddedIdToMovieGenres : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MovieGenreId",
                table: "MovieGenres",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MovieGenreId",
                table: "MovieGenres");
        }
    }
}
