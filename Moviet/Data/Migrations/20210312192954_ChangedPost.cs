using Microsoft.EntityFrameworkCore.Migrations;

namespace Moviet.Data.Migrations
{
    public partial class ChangedPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Movies_MovieId",
                table: "Posts");

            migrationBuilder.AlterColumn<int>(
                name: "MovieId",
                table: "Posts",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Movies_MovieId",
                table: "Posts",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Movies_MovieId",
                table: "Posts");

            migrationBuilder.AlterColumn<int>(
                name: "MovieId",
                table: "Posts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Movies_MovieId",
                table: "Posts",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
