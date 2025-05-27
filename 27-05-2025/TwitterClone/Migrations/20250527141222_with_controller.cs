using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwitterClone.Migrations
{
    /// <inheritdoc />
    public partial class with_controller : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "hash_tags");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "hash_tags");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "users",
                newName: "UserName");

            migrationBuilder.AddColumn<bool>(
                name: "IsLoggedIn",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                table: "users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLoggedIn",
                table: "users");

            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "users",
                newName: "Name");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "hash_tags",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "hash_tags",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
