using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddLessonStatusTable : Migration
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

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Classes");

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

            migrationBuilder.AddColumn<Guid>(
                name: "StatusId",
                table: "Classes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "lessonStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lessonStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pupils_UserId",
                table: "Pupils",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_StatusId",
                table: "Classes",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_lessonStatuses_StatusId",
                table: "Classes",
                column: "StatusId",
                principalTable: "lessonStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_Classes_lessonStatuses_StatusId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Pupils_Users_UserId",
                table: "Pupils");

            migrationBuilder.DropTable(
                name: "lessonStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Pupils_UserId",
                table: "Pupils");

            migrationBuilder.DropIndex(
                name: "IX_Classes_StatusId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Classes");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Pupils",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Classes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

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
