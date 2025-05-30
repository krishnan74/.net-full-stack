using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankApplication.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserId",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionId",
                table: "transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BankId",
                table: "banks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_transactions",
                table: "transactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_banks",
                table: "banks",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_transactions",
                table: "transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_banks",
                table: "banks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserId",
                table: "users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionId",
                table: "transactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankId",
                table: "banks",
                column: "Id");
        }
    }
}
