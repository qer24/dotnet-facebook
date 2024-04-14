using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_facebook.Migrations
{
    /// <inheritdoc />
    public partial class SiteRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "SiteRoles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SiteRoles",
                table: "SiteRoles",
                column: "SiteRoleId");

            migrationBuilder.CreateTable(
                name: "UserSiteRoles",
                columns: table => new
                {
                    UserSiteRoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleSiteRoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSiteRoles", x => x.UserSiteRoleId);
                    table.ForeignKey(
                        name: "FK_UserSiteRoles_SiteRoles_RoleSiteRoleId",
                        column: x => x.RoleSiteRoleId,
                        principalTable: "SiteRoles",
                        principalColumn: "SiteRoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSiteRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSiteRoles_RoleSiteRoleId",
                table: "UserSiteRoles",
                column: "RoleSiteRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSiteRoles_UserId",
                table: "UserSiteRoles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSiteRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SiteRoles",
                table: "SiteRoles");

            migrationBuilder.RenameTable(
                name: "SiteRoles",
                newName: "Roles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "SiteRoleId");
        }
    }
}
