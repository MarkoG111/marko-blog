using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Likes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdComment",
                table: "Likes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Likes_IdComment",
                table: "Likes",
                column: "IdComment");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Comments_IdComment",
                table: "Likes",
                column: "IdComment",
                principalTable: "Comments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Comments_IdComment",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_IdComment",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "IdComment",
                table: "Likes");
        }
    }
}
