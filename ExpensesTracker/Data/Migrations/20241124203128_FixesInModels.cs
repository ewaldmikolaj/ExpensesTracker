using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixesInModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_AspNetUsers_PayerId",
                table: "Expense");

            migrationBuilder.DropColumn(
                name: "ListName",
                table: "ListShare");

            migrationBuilder.AlterColumn<string>(
                name: "PayerId",
                table: "Expense",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_AspNetUsers_PayerId",
                table: "Expense",
                column: "PayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_AspNetUsers_PayerId",
                table: "Expense");

            migrationBuilder.AddColumn<int>(
                name: "ListName",
                table: "ListShare",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "PayerId",
                table: "Expense",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_AspNetUsers_PayerId",
                table: "Expense",
                column: "PayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
