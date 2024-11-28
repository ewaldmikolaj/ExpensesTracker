using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class ListOptionalInExpenseModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_List_ListId",
                table: "Expense");

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "ReceiptPhoto",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "ListId",
                table: "Expense",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_List_ListId",
                table: "Expense",
                column: "ListId",
                principalTable: "List",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_List_ListId",
                table: "Expense");

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "ReceiptPhoto",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ListId",
                table: "Expense",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_List_ListId",
                table: "Expense",
                column: "ListId",
                principalTable: "List",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
