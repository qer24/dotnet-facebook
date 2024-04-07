using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_facebook.Migrations
{
    /// <inheritdoc />
    public partial class RemovePosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupUser_Group_GroupId",
                table: "GroupUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_Group_GroupId",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_Group_ParentGroupGroupId",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Group_GroupId",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Group",
                table: "Group");

            migrationBuilder.RenameTable(
                name: "Group",
                newName: "Groups");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Groups",
                table: "Groups",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupUser_Groups_GroupId",
                table: "GroupUser",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Groups_GroupId",
                table: "Post",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Groups_ParentGroupGroupId",
                table: "Post",
                column: "ParentGroupGroupId",
                principalTable: "Groups",
                principalColumn: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Groups_GroupId",
                table: "Tags",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupUser_Groups_GroupId",
                table: "GroupUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_Groups_GroupId",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_Groups_ParentGroupGroupId",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Groups_GroupId",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Groups",
                table: "Groups");

            migrationBuilder.RenameTable(
                name: "Groups",
                newName: "Group");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Group",
                table: "Group",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupUser_Group_GroupId",
                table: "GroupUser",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Group_GroupId",
                table: "Post",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Group_ParentGroupGroupId",
                table: "Post",
                column: "ParentGroupGroupId",
                principalTable: "Group",
                principalColumn: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Group_GroupId",
                table: "Tags",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "GroupId");
        }
    }
}
