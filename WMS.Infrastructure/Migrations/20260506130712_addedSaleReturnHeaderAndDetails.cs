using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedSaleReturnHeaderAndDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SaleReturnHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleHeaderId = table.Column<int>(type: "int", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalRefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleReturnHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleReturnHeaders_PurchasesHeaders_SaleHeaderId",
                        column: x => x.SaleHeaderId,
                        principalTable: "PurchasesHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SaleReturnDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleReturnHeaderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalRowCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleReturnDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleReturnDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleReturnDetails_SaleReturnHeaders_SaleReturnHeaderId",
                        column: x => x.SaleReturnHeaderId,
                        principalTable: "SaleReturnHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleReturnDetails_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SaleReturnDetails_ProductId",
                table: "SaleReturnDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleReturnDetails_SaleReturnHeaderId",
                table: "SaleReturnDetails",
                column: "SaleReturnHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleReturnDetails_UnitId",
                table: "SaleReturnDetails",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleReturnHeaders_SaleHeaderId",
                table: "SaleReturnHeaders",
                column: "SaleHeaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaleReturnDetails");

            migrationBuilder.DropTable(
                name: "SaleReturnHeaders");
        }
    }
}
