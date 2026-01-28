using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tunepool.Migrations
{
    /// <inheritdoc />
    public partial class removebasemodelonplaylisttags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_PlaylistTags_Playlist_PlaylistId",
            //    table: "PlaylistTags");

            //migrationBuilder.DropIndex(
            //    name: "IX_PlaylistTags_PlaylistId",
            //    table: "PlaylistTags");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PlaylistTags");

            //migrationBuilder.DropColumn(
            //    name: "PlaylistId",
            //    table: "PlaylistTags");

            migrationBuilder.DropColumn(
                name: "createdAt",
                table: "PlaylistTags");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistTags_playlist_id",
                table: "PlaylistTags",
                column: "playlist_id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistTags_Playlist_playlist_id",
                table: "PlaylistTags",
                column: "playlist_id",
                principalTable: "Playlist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_PlaylistTags_Playlist_playlist_id",
            //    table: "PlaylistTags");

            //migrationBuilder.DropIndex(
            //    name: "IX_PlaylistTags_playlist_id",
            //    table: "PlaylistTags");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PlaylistTags",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlaylistId",
                table: "PlaylistTags",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "createdAt",
                table: "PlaylistTags",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistTags_PlaylistId",
                table: "PlaylistTags",
                column: "PlaylistId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistTags_Playlist_PlaylistId",
                table: "PlaylistTags",
                column: "PlaylistId",
                principalTable: "Playlist",
                principalColumn: "Id");
        }
    }
}
