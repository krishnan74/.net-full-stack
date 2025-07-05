using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetAPI.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RazorpayPaymentId",
                table: "payments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RazorpayOrderId",
                table: "orders",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RazorpayPaymentId",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "RazorpayOrderId",
                table: "orders");
        }
    }
}
