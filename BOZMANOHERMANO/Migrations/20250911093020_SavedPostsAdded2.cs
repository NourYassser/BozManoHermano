using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StartUp.Migrations
{
    /// <inheritdoc />
    public partial class SavedPostsAdded2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SavedPosts_Posts_PostsId",
                table: "SavedPosts");

            migrationBuilder.DropIndex(
                name: "IX_SavedPosts_PostsId",
                table: "SavedPosts");

            migrationBuilder.DropColumn(
                name: "PostsId",
                table: "SavedPosts");

            migrationBuilder.CreateIndex(
                name: "IX_SavedPosts_PostId",
                table: "SavedPosts",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_SavedPosts_Posts_PostId",
                table: "SavedPosts",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SavedPosts_Posts_PostId",
                table: "SavedPosts");

            migrationBuilder.DropIndex(
                name: "IX_SavedPosts_PostId",
                table: "SavedPosts");

            migrationBuilder.AddColumn<int>(
                name: "PostsId",
                table: "SavedPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SavedPosts_PostsId",
                table: "SavedPosts",
                column: "PostsId");

            migrationBuilder.AddForeignKey(
                name: "FK_SavedPosts_Posts_PostsId",
                table: "SavedPosts",
                column: "PostsId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
