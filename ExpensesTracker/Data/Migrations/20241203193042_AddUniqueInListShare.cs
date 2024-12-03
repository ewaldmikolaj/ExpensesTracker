using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueInListShare : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ListShare_ListId",
                table: "ListShare");

            migrationBuilder.CreateIndex(
                name: "IX_ListShare_ListId_UserId",
                table: "ListShare",
                columns: new[] { "ListId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ListShare_ListId_UserId",
                table: "ListShare");

            migrationBuilder.CreateIndex(
                name: "IX_ListShare_ListId",
                table: "ListShare",
                column: "ListId");
        }
    }
}
