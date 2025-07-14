using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace QuizupAPI.Migrations
{
    /// <inheritdoc />
    public partial class Summary_DbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "studentSummary",
                columns: table => new
                {
                    StudentId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentName = table.Column<string>(type: "text", nullable: false),
                    StudentEmail = table.Column<string>(type: "text", nullable: false),
                    StudentClass = table.Column<string>(type: "text", nullable: false),
                    TotalQuizzesAvailable = table.Column<long>(type: "bigint", nullable: false),
                    TotalQuizzesStarted = table.Column<long>(type: "bigint", nullable: false),
                    TotalQuizzesCompleted = table.Column<long>(type: "bigint", nullable: false),
                    TotalQuizzesInProgress = table.Column<long>(type: "bigint", nullable: false),
                    TotalQuizzesSaved = table.Column<long>(type: "bigint", nullable: false),
                    AverageScore = table.Column<decimal>(type: "numeric", nullable: false),
                    HighestScore = table.Column<int>(type: "integer", nullable: false),
                    LowestScore = table.Column<int>(type: "integer", nullable: false),
                    TotalQuestionsAttempted = table.Column<long>(type: "bigint", nullable: false),
                    TotalCorrectAnswers = table.Column<long>(type: "bigint", nullable: false),
                    AccuracyPercentage = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalTimeSpentMinutes = table.Column<int>(type: "integer", nullable: false),
                    QuizzesByStatus = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    RecentActivity = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    PerformanceTrend = table.Column<JsonDocument>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studentSummary", x => x.StudentId);
                });

            migrationBuilder.CreateTable(
                name: "teacherSummary",
                columns: table => new
                {
                    TeacherId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TeacherName = table.Column<string>(type: "text", nullable: false),
                    TeacherEmail = table.Column<string>(type: "text", nullable: false),
                    TeacherSubject = table.Column<string>(type: "text", nullable: false),
                    TotalQuizzesCreated = table.Column<long>(type: "bigint", nullable: false),
                    TotalActiveQuizzes = table.Column<long>(type: "bigint", nullable: false),
                    TotalInactiveQuizzes = table.Column<long>(type: "bigint", nullable: false),
                    TotalQuestionsCreated = table.Column<long>(type: "bigint", nullable: false),
                    TotalStudentSubmissions = table.Column<long>(type: "bigint", nullable: false),
                    TotalStudentsParticipated = table.Column<long>(type: "bigint", nullable: false),
                    AverageCompletionRate = table.Column<decimal>(type: "numeric", nullable: false),
                    AverageStudentScore = table.Column<decimal>(type: "numeric", nullable: false),
                    HighestQuizScore = table.Column<int>(type: "integer", nullable: false),
                    LowestQuizScore = table.Column<int>(type: "integer", nullable: false),
                    TotalQuestionsAnswered = table.Column<long>(type: "bigint", nullable: false),
                    TotalCorrectAnswers = table.Column<long>(type: "bigint", nullable: false),
                    OverallAccuracyPercentage = table.Column<decimal>(type: "numeric", nullable: false),
                    QuizzesByStatus = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    StudentPerformanceSummary = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    RecentQuizActivity = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    QuizPerformanceTrend = table.Column<JsonDocument>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacherSummary", x => x.TeacherId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "studentSummary");

            migrationBuilder.DropTable(
                name: "teacherSummary");
        }
    }
}
