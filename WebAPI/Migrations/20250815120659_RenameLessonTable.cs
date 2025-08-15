using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameLessonTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_lessonStatuses_StatusId",
                table: "Classes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_lessonStatuses",
                table: "lessonStatuses");

            migrationBuilder.RenameTable(
                name: "lessonStatuses",
                newName: "LessonStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LessonStatuses",
                table: "LessonStatuses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_LessonStatuses_StatusId",
                table: "Classes",
                column: "StatusId",
                principalTable: "LessonStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_LessonStatuses_StatusId",
                table: "Classes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LessonStatuses",
                table: "LessonStatuses");

            migrationBuilder.RenameTable(
                name: "LessonStatuses",
                newName: "lessonStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_lessonStatuses",
                table: "lessonStatuses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_lessonStatuses_StatusId",
                table: "Classes",
                column: "StatusId",
                principalTable: "lessonStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
