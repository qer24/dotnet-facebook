using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_facebook.Migrations
{
    /// <inheritdoc />
    public partial class PostsRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Post_PostId1",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "PostId1",
                table: "Post",
                newName: "MainPostPostId");

            migrationBuilder.RenameIndex(
                name: "IX_Post_PostId1",
                table: "Post",
                newName: "IX_Post_MainPostPostId");

            migrationBuilder.AddColumn<string>(
                name: "PostLocation",
                table: "Post",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Post_MainPostPostId",
                table: "Post",
                column: "MainPostPostId",
                principalTable: "Post",
                principalColumn: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Post_MainPostPostId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "PostLocation",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "MainPostPostId",
                table: "Post",
                newName: "PostId1");

            migrationBuilder.RenameIndex(
                name: "IX_Post_MainPostPostId",
                table: "Post",
                newName: "IX_Post_PostId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Post_PostId1",
                table: "Post",
                column: "PostId1",
                principalTable: "Post",
                principalColumn: "PostId");
        }
    }
}
