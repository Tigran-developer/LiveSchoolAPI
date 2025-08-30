using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class cursorupdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "durationInMinutes",
                table: "Classes",
                newName: "DurationInMinutes");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Classes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "MaxParticipants",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Classes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DifficultyId",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Classes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsOnline",
                table: "Classes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Classes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Materials",
                table: "Classes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Classes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Classes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "RecordingUrl",
                table: "Classes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Classes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Classes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Classes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ClassDifficulties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassDifficulties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassStatuses", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ClassDifficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "BEGINNER" },
                    { 2, "INTERMEDIATE" },
                    { 3, "ADVANCED" },
                    { 4, "EXPERT" }
                });

            migrationBuilder.InsertData(
                table: "ClassStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "DRAFT" },
                    { 2, "PUBLISHED" },
                    { 3, "IN_PROGRESS" },
                    { 4, "COMPLETED" },
                    { 5, "CANCELLED" },
                    { 6, "ARCHIVED" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_DifficultyId",
                table: "Classes",
                column: "DifficultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_ClassDifficulties_DifficultyId",
                table: "Classes",
                column: "DifficultyId",
                principalTable: "ClassDifficulties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_ClassDifficulties_DifficultyId",
                table: "Classes");

            migrationBuilder.DropTable(
                name: "ClassDifficulties");

            migrationBuilder.DropTable(
                name: "ClassStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Classes_DifficultyId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "DifficultyId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "IsOnline",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "Materials",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "RecordingUrl",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Classes");

            migrationBuilder.RenameColumn(
                name: "DurationInMinutes",
                table: "Classes",
                newName: "durationInMinutes");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Classes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "RecurrencePattern",
                table: "Classes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaxParticipants",
                table: "Classes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Classes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);
        }
    }
}
