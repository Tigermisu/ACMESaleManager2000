using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ACMESaleManager2000.Data.Migrations
{
    public partial class RenamedEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemPurchaseOrder");

            migrationBuilder.DropTable(
                name: "ItemSaleOrder");

            migrationBuilder.CreateTable(
                name: "ItemPurchaseOrderEntity",
                columns: table => new
                {
                    ItemEntityId = table.Column<int>(nullable: false),
                    PurchaseOrderEntityId = table.Column<int>(nullable: false),
                    PurchasedPrice = table.Column<decimal>(nullable: false),
                    PurchasedQuantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemPurchaseOrderEntity", x => new { x.ItemEntityId, x.PurchaseOrderEntityId });
                    table.ForeignKey(
                        name: "FK_ItemPurchaseOrderEntity_Items_ItemEntityId",
                        column: x => x.ItemEntityId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemPurchaseOrderEntity_PurchaseOrders_PurchaseOrderEntityId",
                        column: x => x.PurchaseOrderEntityId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemSaleOrderEntity",
                columns: table => new
                {
                    ItemEntityId = table.Column<int>(nullable: false),
                    SaleOrderEntityId = table.Column<int>(nullable: false),
                    SoldPrice = table.Column<decimal>(nullable: false),
                    SoldQuantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemSaleOrderEntity", x => new { x.ItemEntityId, x.SaleOrderEntityId });
                    table.ForeignKey(
                        name: "FK_ItemSaleOrderEntity_Items_ItemEntityId",
                        column: x => x.ItemEntityId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemSaleOrderEntity_SaleOrders_SaleOrderEntityId",
                        column: x => x.SaleOrderEntityId,
                        principalTable: "SaleOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemPurchaseOrderEntity_PurchaseOrderEntityId",
                table: "ItemPurchaseOrderEntity",
                column: "PurchaseOrderEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSaleOrderEntity_SaleOrderEntityId",
                table: "ItemSaleOrderEntity",
                column: "SaleOrderEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemPurchaseOrderEntity");

            migrationBuilder.DropTable(
                name: "ItemSaleOrderEntity");

            migrationBuilder.CreateTable(
                name: "ItemPurchaseOrder",
                columns: table => new
                {
                    ItemEntityId = table.Column<int>(nullable: false),
                    PurchaseOrderEntityId = table.Column<int>(nullable: false),
                    PurchasedPrice = table.Column<decimal>(nullable: false),
                    PurchasedQuantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemPurchaseOrder", x => new { x.ItemEntityId, x.PurchaseOrderEntityId });
                    table.ForeignKey(
                        name: "FK_ItemPurchaseOrder_Items_ItemEntityId",
                        column: x => x.ItemEntityId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemPurchaseOrder_PurchaseOrders_PurchaseOrderEntityId",
                        column: x => x.PurchaseOrderEntityId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemSaleOrder",
                columns: table => new
                {
                    ItemEntityId = table.Column<int>(nullable: false),
                    SaleOrderEntityId = table.Column<int>(nullable: false),
                    SoldPrice = table.Column<decimal>(nullable: false),
                    SoldQuantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemSaleOrder", x => new { x.ItemEntityId, x.SaleOrderEntityId });
                    table.ForeignKey(
                        name: "FK_ItemSaleOrder_Items_ItemEntityId",
                        column: x => x.ItemEntityId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemSaleOrder_SaleOrders_SaleOrderEntityId",
                        column: x => x.SaleOrderEntityId,
                        principalTable: "SaleOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemPurchaseOrder_PurchaseOrderEntityId",
                table: "ItemPurchaseOrder",
                column: "PurchaseOrderEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSaleOrder_SaleOrderEntityId",
                table: "ItemSaleOrder",
                column: "SaleOrderEntityId");
        }
    }
}
