using Microsoft.EntityFrameworkCore.Migrations;

namespace Moviet.Data.Migrations
{
    public partial class UndoLast : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MovieGenreId",
                table: "MovieGenres");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MovieGenreId",
                table: "MovieGenres",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
