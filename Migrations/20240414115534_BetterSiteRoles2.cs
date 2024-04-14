using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_facebook.Migrations
{
    /// <inheritdoc />
    public partial class BetterSiteRoles2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdministrativePerms",
                table: "UserSiteRoles");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "UserSiteRoles");

            migrationBuilder.AddColumn<bool>(
                name: "AdministrativePerms",
                table: "SiteRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "SiteRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdministrativePerms",
                table: "SiteRoles");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "SiteRoles");

            migrationBuilder.AddColumn<bool>(
                name: "AdministrativePerms",
                table: "UserSiteRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "UserSiteRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
