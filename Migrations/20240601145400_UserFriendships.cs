using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_facebook.Migrations
{
    /// <inheritdoc />
    public partial class UserFriendships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Users_UserId",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateMessages_Friendships_FriendshipId",
                table: "PrivateMessages");

            migrationBuilder.DropIndex(
                name: "IX_PrivateMessages_FriendshipId",
                table: "PrivateMessages");

            migrationBuilder.DropIndex(
                name: "IX_Friendships_UserId",
                table: "Friendships");

            migrationBuilder.DropColumn(
                name: "FriendshipId",
                table: "PrivateMessages");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Friendships");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FriendshipId",
                table: "PrivateMessages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Friendships",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMessages_FriendshipId",
                table: "PrivateMessages",
                column: "FriendshipId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_UserId",
                table: "Friendships",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Users_UserId",
                table: "Friendships",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateMessages_Friendships_FriendshipId",
                table: "PrivateMessages",
                column: "FriendshipId",
                principalTable: "Friendships",
                principalColumn: "FriendshipId");
        }
    }
}
