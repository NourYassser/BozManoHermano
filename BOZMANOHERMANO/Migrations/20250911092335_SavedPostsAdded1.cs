using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StartUp.Migrations
{
    /// <inheritdoc />
    public partial class SavedPostsAdded1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_SavedPosts_SavedPostsId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_SavedPostsId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "SavedPostsId",
                table: "Posts");

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
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "SavedPostsId",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_SavedPostsId",
                table: "Posts",
                column: "SavedPostsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_SavedPosts_SavedPostsId",
                table: "Posts",
                column: "SavedPostsId",
                principalTable: "SavedPosts",
                principalColumn: "Id");
        }
    }
}
