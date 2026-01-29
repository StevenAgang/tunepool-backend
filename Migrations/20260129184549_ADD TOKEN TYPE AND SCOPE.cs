using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tunepool.Migrations
{
    /// <inheritdoc />
    public partial class ADDTOKENTYPEANDSCOPE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SCscope",
                table: "ServiceProviderToken",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tokenType",
                table: "ServiceProviderToken",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SCscope",
                table: "ServiceProviderToken");

            migrationBuilder.DropColumn(
                name: "tokenType",
                table: "ServiceProviderToken");
        }
    }
}
