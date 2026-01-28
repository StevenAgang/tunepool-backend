using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tunepool.Migrations
{
    /// <inheritdoc />
    public partial class fixmoderelationshipsissue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Popularity_playListId",
                table: "Popularity");

            migrationBuilder.DropIndex(
                name: "IX_PlaylistTags_playlist_id",
                table: "PlaylistTags");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Popularity",
                table: "Popularity",
                column: "playListId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaylistTags",
                table: "PlaylistTags",
                columns: new[] { "playlist_id", "tags_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Popularity",
                table: "Popularity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaylistTags",
                table: "PlaylistTags");

            migrationBuilder.CreateIndex(
                name: "IX_Popularity_playListId",
                table: "Popularity",
                column: "playListId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistTags_playlist_id",
                table: "PlaylistTags",
                column: "playlist_id");
        }
    }
}
