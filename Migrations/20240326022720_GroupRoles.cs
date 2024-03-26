using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_facebook.Migrations
{
    /// <inheritdoc />
    public partial class GroupRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Group_Users_OwnerUserUserId",
                table: "Group");

            migrationBuilder.DropIndex(
                name: "IX_Group_OwnerUserUserId",
                table: "Group");

            migrationBuilder.DropColumn(
                name: "IsModerator",
                table: "GroupUser");

            migrationBuilder.DropColumn(
                name: "OwnerUserUserId",
                table: "Group");

            migrationBuilder.AddColumn<int>(
                name: "GroupRole",
                table: "GroupUser",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupRole",
                table: "GroupUser");

            migrationBuilder.AddColumn<bool>(
                name: "IsModerator",
                table: "GroupUser",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OwnerUserUserId",
                table: "Group",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Group_OwnerUserUserId",
                table: "Group",
                column: "OwnerUserUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Group_Users_OwnerUserUserId",
                table: "Group",
                column: "OwnerUserUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
