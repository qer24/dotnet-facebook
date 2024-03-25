using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_facebook.Migrations
{
    /// <inheritdoc />
    public partial class test2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostGeolocation",
                table: "Post",
                newName: "PostLongitude");

            migrationBuilder.AddColumn<int>(
                name: "PostLatitude",
                table: "Post",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostLatitude",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "PostLongitude",
                table: "Post",
                newName: "PostGeolocation");
        }
    }
}
