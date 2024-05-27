using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_facebook.Migrations
{
    /// <inheritdoc />
    public partial class PostsRefactorRemoveCoordinates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostLatitude",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "PostLongitude",
                table: "Post");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostLatitude",
                table: "Post",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PostLongitude",
                table: "Post",
                type: "int",
                nullable: true);
        }
    }
}
