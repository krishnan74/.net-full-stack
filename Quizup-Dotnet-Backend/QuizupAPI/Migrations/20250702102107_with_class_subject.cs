using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace QuizupAPI.Migrations
{
    /// <inheritdoc />
    public partial class with_class_subject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject",
                table: "teachers");

            migrationBuilder.DropColumn(
                name: "Class",
                table: "students");

            migrationBuilder.AddColumn<long>(
                name: "ClassId",
                table: "students",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ClassId",
                table: "quizzes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SubjectId",
                table: "quizzes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string[]>(
                name: "Tags",
                table: "quizzes",
                type: "text[]",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "classes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "subjects",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "teacherClasses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TeacherId = table.Column<long>(type: "bigint", nullable: false),
                    ClassId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacherClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherClass_Class",
                        column: x => x.ClassId,
                        principalTable: "classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherClass_Teacher",
                        column: x => x.TeacherId,
                        principalTable: "teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "classSubjects",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClassId = table.Column<long>(type: "bigint", nullable: false),
                    SubjectId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassSubject_Class",
                        column: x => x.ClassId,
                        principalTable: "classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassSubject_Subject",
                        column: x => x.SubjectId,
                        principalTable: "subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "teacherSubjects",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TeacherId = table.Column<long>(type: "bigint", nullable: false),
                    SubjectId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacherSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherSubject_Subject",
                        column: x => x.SubjectId,
                        principalTable: "subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherSubject_Teacher",
                        column: x => x.TeacherId,
                        principalTable: "teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_students_ClassId",
                table: "students",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_quizzes_ClassId",
                table: "quizzes",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_quizzes_SubjectId",
                table: "quizzes",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_classSubjects_ClassId",
                table: "classSubjects",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_classSubjects_SubjectId",
                table: "classSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_teacherClasses_ClassId",
                table: "teacherClasses",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_teacherClasses_TeacherId",
                table: "teacherClasses",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_teacherSubjects_SubjectId",
                table: "teacherSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_teacherSubjects_TeacherId",
                table: "teacherSubjects",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quiz_Class",
                table: "quizzes",
                column: "ClassId",
                principalTable: "classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Quiz_Subject",
                table: "quizzes",
                column: "SubjectId",
                principalTable: "subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Class",
                table: "students",
                column: "ClassId",
                principalTable: "classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quiz_Class",
                table: "quizzes");

            migrationBuilder.DropForeignKey(
                name: "FK_Quiz_Subject",
                table: "quizzes");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Class",
                table: "students");

            migrationBuilder.DropTable(
                name: "classSubjects");

            migrationBuilder.DropTable(
                name: "teacherClasses");

            migrationBuilder.DropTable(
                name: "teacherSubjects");

            migrationBuilder.DropTable(
                name: "classes");

            migrationBuilder.DropTable(
                name: "subjects");

            migrationBuilder.DropIndex(
                name: "IX_students_ClassId",
                table: "students");

            migrationBuilder.DropIndex(
                name: "IX_quizzes_ClassId",
                table: "quizzes");

            migrationBuilder.DropIndex(
                name: "IX_quizzes_SubjectId",
                table: "quizzes");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "students");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "quizzes");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "quizzes");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "quizzes");

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "teachers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Class",
                table: "students",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
