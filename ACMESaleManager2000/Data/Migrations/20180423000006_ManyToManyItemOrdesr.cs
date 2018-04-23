using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ACMESaleManager2000.Data.Migrations
{
    public partial class ManyToManyItemOrdesr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Items_ItemEntityId",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_SaleOrders_Items_ItemEntityId",
                table: "SaleOrders");

            migrationBuilder.DropIndex(
                name: "IX_SaleOrders_ItemEntityId",
                table: "SaleOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_ItemEntityId",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ItemEntityId",
                table: "SaleOrders");

            migrationBuilder.DropColumn(
                name: "SoldQuantity",
                table: "SaleOrders");

            migrationBuilder.DropColumn(
                name: "ItemEntityId",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "PurchasedQuantity",
                table: "PurchaseOrders");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemPurchaseOrder");

            migrationBuilder.DropTable(
                name: "ItemSaleOrder");

            migrationBuilder.AddColumn<int>(
                name: "ItemEntityId",
                table: "SaleOrders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SoldQuantity",
                table: "SaleOrders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemEntityId",
                table: "PurchaseOrders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PurchasedQuantity",
                table: "PurchaseOrders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SaleOrders_ItemEntityId",
                table: "SaleOrders",
                column: "ItemEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_ItemEntityId",
                table: "PurchaseOrders",
                column: "ItemEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Items_ItemEntityId",
                table: "PurchaseOrders",
                column: "ItemEntityId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SaleOrders_Items_ItemEntityId",
                table: "SaleOrders",
                column: "ItemEntityId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
