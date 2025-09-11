using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StartUp.Migrations
{
    /// <inheritdoc />
    public partial class SavedPostsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SavedPostsId",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SavedPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedPosts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_SavedPostsId",
                table: "Posts",
                column: "SavedPostsId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedPosts_UserId",
                table: "SavedPosts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_SavedPosts_SavedPostsId",
                table: "Posts",
                column: "SavedPostsId",
                principalTable: "SavedPosts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_SavedPosts_SavedPostsId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "SavedPosts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_SavedPostsId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "SavedPostsId",
                table: "Posts");
        }
    }
}
