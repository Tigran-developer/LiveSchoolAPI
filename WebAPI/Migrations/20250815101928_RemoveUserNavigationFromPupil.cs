using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserNavigationFromPupil : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pupils_Users_UserId",
                table: "Pupils");

            migrationBuilder.DropIndex(
                name: "IX_Pupils_UserId",
                table: "Pupils");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Pupils",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pupils_UserId",
                table: "Pupils",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pupils_Users_UserId",
                table: "Pupils",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pupils_Users_UserId",
                table: "Pupils");

            migrationBuilder.DropIndex(
                name: "IX_Pupils_UserId",
                table: "Pupils");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Pupils",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.CreateIndex(
                name: "IX_Pupils_UserId",
                table: "Pupils",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Pupils_Users_UserId",
                table: "Pupils",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
