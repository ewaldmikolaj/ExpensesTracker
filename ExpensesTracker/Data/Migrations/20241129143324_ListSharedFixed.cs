using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class ListSharedFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListShare_List_ListId",
                table: "ListShare");

            migrationBuilder.AlterColumn<int>(
                name: "ListId",
                table: "ListShare",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_ListShare_List_ListId",
                table: "ListShare",
                column: "ListId",
                principalTable: "List",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListShare_List_ListId",
                table: "ListShare");

            migrationBuilder.AlterColumn<int>(
                name: "ListId",
                table: "ListShare",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ListShare_List_ListId",
                table: "ListShare",
                column: "ListId",
                principalTable: "List",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
