using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tunepool.Migrations
{
    /// <inheritdoc />
    public partial class REMOVESCOPEANDTOKENTYPEANDADDPLATFORMFOREIGNKEY : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SCaccessToken",
                table: "ServiceProviderToken");

            migrationBuilder.DropColumn(
                name: "SCrefreshToken",
                table: "ServiceProviderToken");

            migrationBuilder.RenameColumn(
                name: "tokenType",
                table: "ServiceProviderToken",
                newName: "refreshToken");

            migrationBuilder.RenameColumn(
                name: "SCscope",
                table: "ServiceProviderToken",
                newName: "accessToken");

            migrationBuilder.RenameColumn(
                name: "SCexpiresIn",
                table: "ServiceProviderToken",
                newName: "expiresIn");

            migrationBuilder.AddColumn<int>(
                name: "platformId",
                table: "ServiceProviderToken",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceProviderToken_platformId",
                table: "ServiceProviderToken",
                column: "platformId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceProviderToken_PlatForm_platformId",
                table: "ServiceProviderToken",
                column: "platformId",
                principalTable: "PlatForm",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceProviderToken_PlatForm_platformId",
                table: "ServiceProviderToken");

            migrationBuilder.DropIndex(
                name: "IX_ServiceProviderToken_platformId",
                table: "ServiceProviderToken");

            migrationBuilder.DropColumn(
                name: "platformId",
                table: "ServiceProviderToken");

            migrationBuilder.RenameColumn(
                name: "refreshToken",
                table: "ServiceProviderToken",
                newName: "tokenType");

            migrationBuilder.RenameColumn(
                name: "expiresIn",
                table: "ServiceProviderToken",
                newName: "SCexpiresIn");

            migrationBuilder.RenameColumn(
                name: "accessToken",
                table: "ServiceProviderToken",
                newName: "SCscope");

            migrationBuilder.AddColumn<string>(
                name: "SCaccessToken",
                table: "ServiceProviderToken",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SCrefreshToken",
                table: "ServiceProviderToken",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
