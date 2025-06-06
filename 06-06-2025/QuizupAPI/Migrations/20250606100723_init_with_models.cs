using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace QuizupAPI.Migrations
{
    /// <inheritdoc />
    public partial class init_with_models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Username = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<byte[]>(type: "bytea", nullable: true),
                    HashKey = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Class = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Student",
                        column: x => x.Email,
                        principalTable: "users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "teachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Subject = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Teacher",
                        column: x => x.Email,
                        principalTable: "users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "quizzes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    TeacherId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quizzes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quiz_Teacher",
                        column: x => x.TeacherId,
                        principalTable: "teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "text", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "text", nullable: false),
                    Options = table.Column<List<string>>(type: "text[]", nullable: false),
                    QuizId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Question_Quiz",
                        column: x => x.QuizId,
                        principalTable: "quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "quizSubmissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuizId = table.Column<int>(type: "integer", nullable: false),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quizSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizSubmission_Quiz",
                        column: x => x.QuizId,
                        principalTable: "quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuizSubmission_Student",
                        column: x => x.StudentId,
                        principalTable: "students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "answers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    QuizSubmissionId = table.Column<int>(type: "integer", nullable: false),
                    SelectedAnswer = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answer_Question",
                        column: x => x.QuestionId,
                        principalTable: "questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Answer_QuizSubmission",
                        column: x => x.QuizSubmissionId,
                        principalTable: "quizSubmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_answers_QuestionId",
                table: "answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_answers_QuizSubmissionId",
                table: "answers",
                column: "QuizSubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_questions_QuizId",
                table: "questions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_quizSubmissions_QuizId",
                table: "quizSubmissions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_quizSubmissions_StudentId",
                table: "quizSubmissions",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_quizzes_TeacherId",
                table: "quizzes",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_students_Email",
                table: "students",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_teachers_Email",
                table: "teachers",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "answers");

            migrationBuilder.DropTable(
                name: "questions");

            migrationBuilder.DropTable(
                name: "quizSubmissions");

            migrationBuilder.DropTable(
                name: "quizzes");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "teachers");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
