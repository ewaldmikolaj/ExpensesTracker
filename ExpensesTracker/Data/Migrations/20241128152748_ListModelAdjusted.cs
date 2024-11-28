using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class ListModelAdjusted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_List_ListId",
                table: "Expense");

            migrationBuilder.DropForeignKey(
                name: "FK_List_AspNetUsers_OwnerId",
                table: "List");

            migrationBuilder.DropForeignKey(
                name: "FK_ListShare_AspNetUsers_UserId",
                table: "ListShare");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ListShare",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "List",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_List_ListId",
                table: "Expense",
                column: "ListId",
                principalTable: "List",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_List_AspNetUsers_OwnerId",
                table: "List",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ListShare_AspNetUsers_UserId",
                table: "ListShare",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_List_ListId",
                table: "Expense");

            migrationBuilder.DropForeignKey(
                name: "FK_List_AspNetUsers_OwnerId",
                table: "List");

            migrationBuilder.DropForeignKey(
                name: "FK_ListShare_AspNetUsers_UserId",
                table: "ListShare");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ListShare",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "List",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_List_ListId",
                table: "Expense",
                column: "ListId",
                principalTable: "List",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_List_AspNetUsers_OwnerId",
                table: "List",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ListShare_AspNetUsers_UserId",
                table: "ListShare",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
