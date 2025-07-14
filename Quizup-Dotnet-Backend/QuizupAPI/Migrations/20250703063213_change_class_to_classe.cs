using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizupAPI.Migrations
{
    /// <inheritdoc />
    public partial class change_class_to_classe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeacherSubject",
                table: "teacherSummary");

            migrationBuilder.AddColumn<JsonDocument>(
                name: "TeacherSubjects",
                table: "teacherSummary",
                type: "jsonb",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeacherSubjects",
                table: "teacherSummary");

            migrationBuilder.AddColumn<string>(
                name: "TeacherSubject",
                table: "teacherSummary",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
