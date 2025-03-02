using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SetDeleteRestrict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_IdUser",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Users_IdUser",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_FromIdUser",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_PostCategories_Posts_IdPost",
                table: "PostCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_IdUser",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_IdRole",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "FromIdUser",
                table: "Notifications",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_IdUser",
                table: "Comments",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Users_IdUser",
                table: "Likes",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_FromIdUser",
                table: "Notifications",
                column: "FromIdUser",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategories_Posts_IdPost",
                table: "PostCategories",
                column: "IdPost",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_IdUser",
                table: "Posts",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_IdRole",
                table: "Users",
                column: "IdRole",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_IdUser",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Users_IdUser",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_FromIdUser",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_PostCategories_Posts_IdPost",
                table: "PostCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_IdUser",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_IdRole",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<int>(
                name: "FromIdUser",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_IdUser",
                table: "Comments",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Users_IdUser",
                table: "Likes",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_FromIdUser",
                table: "Notifications",
                column: "FromIdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategories_Posts_IdPost",
                table: "PostCategories",
                column: "IdPost",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_IdUser",
                table: "Posts",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_IdRole",
                table: "Users",
                column: "IdRole",
                principalTable: "Roles",
                principalColumn: "Id");
        }
    }
}
