using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedPurchasesReturnHeaderAndDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchasesReturnHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseHeaderId = table.Column<int>(type: "int", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalRefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasesReturnHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchasesReturnHeaders_PurchasesHeaders_PurchaseHeaderId",
                        column: x => x.PurchaseHeaderId,
                        principalTable: "PurchasesHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchasesReturnDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchasesReturnHeaderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalRowCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasesReturnDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchasesReturnDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchasesReturnDetails_PurchasesReturnHeaders_PurchasesReturnHeaderId",
                        column: x => x.PurchasesReturnHeaderId,
                        principalTable: "PurchasesReturnHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchasesReturnDetails_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchasesReturnDetails_ProductId",
                table: "PurchasesReturnDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasesReturnDetails_PurchasesReturnHeaderId",
                table: "PurchasesReturnDetails",
                column: "PurchasesReturnHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasesReturnDetails_UnitId",
                table: "PurchasesReturnDetails",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasesReturnHeaders_PurchaseHeaderId",
                table: "PurchasesReturnHeaders",
                column: "PurchaseHeaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchasesReturnDetails");

            migrationBuilder.DropTable(
                name: "PurchasesReturnHeaders");
        }
    }
}
