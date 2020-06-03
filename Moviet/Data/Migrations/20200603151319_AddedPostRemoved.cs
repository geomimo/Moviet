using Microsoft.EntityFrameworkCore.Migrations;

namespace Moviet.Data.Migrations
{
    public partial class AddedPostRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PostRemoved",
                table: "Movies",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostRemoved",
                table: "Movies");
        }
    }
}
