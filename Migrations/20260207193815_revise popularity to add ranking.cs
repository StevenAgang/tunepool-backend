using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tunepool.Migrations
{
    /// <inheritdoc />
    public partial class revisepopularitytoaddranking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "rank",
                table: "Popularity",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rank",
                table: "Popularity");
        }
    }
}
