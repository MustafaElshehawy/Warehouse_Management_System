using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SolveRelationPursaceheaderWithdetaials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasesDetails_PurchasesHeaders_PurchasesHeaderId",
                table: "PurchasesDetails");

            migrationBuilder.DropIndex(
                name: "IX_PurchasesDetails_PurchasesHeaderId",
                table: "PurchasesDetails");

            migrationBuilder.DropColumn(
                name: "PurchasesHeaderId",
                table: "PurchasesDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PurchasesHeaderId",
                table: "PurchasesDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchasesDetails_PurchasesHeaderId",
                table: "PurchasesDetails",
                column: "PurchasesHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasesDetails_PurchasesHeaders_PurchasesHeaderId",
                table: "PurchasesDetails",
                column: "PurchasesHeaderId",
                principalTable: "PurchasesHeaders",
                principalColumn: "Id");
        }
    }
}
