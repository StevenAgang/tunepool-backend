using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tunepool.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlatForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatForm", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Playlist",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    playList_Urls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    platform_id = table.Column<int>(type: "int", nullable: false),
                    thumbnail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Playlist_PlatForm_platform_id",
                        column: x => x.platform_id,
                        principalTable: "PlatForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistTags",
                columns: table => new
                {
                    playlist_id = table.Column<int>(type: "int", nullable: false),
                    tags_id = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_PlaylistTags_Playlist_PlaylistId",
                        column: x => x.playlist_id,
                        principalTable: "Playlist",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlaylistTags_Tags_tags_id",
                        column: x => x.tags_id,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Popularity",
                columns: table => new
                {
                    playListId = table.Column<int>(type: "int", nullable: false),
                    likes = table.Column<int>(type: "int", nullable: false),
                    hearts = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_Popularity_Playlist_playListId",
                        column: x => x.playListId,
                        principalTable: "Playlist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Playlist_platform_id",
                table: "Playlist",
                column: "platform_id");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistTags_PlaylistId",
                table: "PlaylistTags",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistTags_tags_id",
                table: "PlaylistTags",
                column: "tags_id");

            migrationBuilder.CreateIndex(
                name: "IX_Popularity_playListId",
                table: "Popularity",
                column: "playListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaylistTags");

            migrationBuilder.DropTable(
                name: "Popularity");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Playlist");

            migrationBuilder.DropTable(
                name: "PlatForm");
        }
    }
}
