using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_facebook.Migrations
{
    /// <inheritdoc />
    public partial class Comments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Post_MainPostPostId",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "MainPostPostId",
                table: "Post",
                newName: "ParentPostPostId");

            migrationBuilder.RenameIndex(
                name: "IX_Post_MainPostPostId",
                table: "Post",
                newName: "IX_Post_ParentPostPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Post_ParentPostPostId",
                table: "Post",
                column: "ParentPostPostId",
                principalTable: "Post",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Post_ParentPostPostId",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "ParentPostPostId",
                table: "Post",
                newName: "MainPostPostId");

            migrationBuilder.RenameIndex(
                name: "IX_Post_ParentPostPostId",
                table: "Post",
                newName: "IX_Post_MainPostPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Post_MainPostPostId",
                table: "Post",
                column: "MainPostPostId",
                principalTable: "Post",
                principalColumn: "PostId");
        }
    }
}
