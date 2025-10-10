using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastTrack.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kiosks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedBy = table.Column<int>(type: "int", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kiosks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KioskId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sku = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedBy = table.Column<int>(type: "int", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Kiosks_KioskId",
                        column: x => x.KioskId,
                        principalTable: "Kiosks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalOrderkey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalLines = table.Column<int>(type: "int", nullable: false),
                    TotalQuantity = table.Column<int>(type: "int", nullable: false),
                    KioskId = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedBy = table.Column<int>(type: "int", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Kiosks_KioskId",
                        column: x => x.KioskId,
                        principalTable: "Kiosks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CurrentInventories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    KioskId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedBy = table.Column<int>(type: "int", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    KioskModelId = table.Column<int>(type: "int", nullable: true),
                    ProductModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentInventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrentInventories_Kiosks_KioskId",
                        column: x => x.KioskId,
                        principalTable: "Kiosks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CurrentInventories_Kiosks_KioskModelId",
                        column: x => x.KioskModelId,
                        principalTable: "Kiosks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CurrentInventories_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CurrentInventories_Products_ProductModelId",
                        column: x => x.ProductModelId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InventoryMovements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkuCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    InventoryMovementType = table.Column<int>(type: "int", nullable: false),
                    KioskId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedBy = table.Column<int>(type: "int", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryMovements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryMovements_Kiosks_KioskId",
                        column: x => x.KioskId,
                        principalTable: "Kiosks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryMovements_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InventoryMovements_PurchaseOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Line = table.Column<int>(type: "int", nullable: false),
                    SkuCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    PurchaseOrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedBy = table.Column<int>(type: "int", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false),
                    ProductModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductModelId",
                        column: x => x.ProductModelId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderDetails_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrentInventories_KioskId",
                table: "CurrentInventories",
                column: "KioskId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentInventories_KioskModelId",
                table: "CurrentInventories",
                column: "KioskModelId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentInventories_ProductId_KioskId",
                table: "CurrentInventories",
                columns: new[] { "ProductId", "KioskId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CurrentInventories_ProductModelId",
                table: "CurrentInventories",
                column: "ProductModelId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryMovements_KioskId_ProductId",
                table: "InventoryMovements",
                columns: new[] { "KioskId", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryMovements_OrderId",
                table: "InventoryMovements",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryMovements_ProductId_DateAdded",
                table: "InventoryMovements",
                columns: new[] { "ProductId", "DateAdded" });

            migrationBuilder.CreateIndex(
                name: "IX_Kiosks_Code",
                table: "Kiosks",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductModelId",
                table: "OrderDetails",
                column: "ProductModelId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_PurchaseOrderId_ProductId",
                table: "OrderDetails",
                columns: new[] { "PurchaseOrderId", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_KioskId",
                table: "Products",
                column: "KioskId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Sku",
                table: "Products",
                column: "Sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_DateAdded",
                table: "PurchaseOrders",
                column: "DateAdded");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_KioskId_Status",
                table: "PurchaseOrders",
                columns: new[] { "KioskId", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrentInventories");

            migrationBuilder.DropTable(
                name: "InventoryMovements");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "Kiosks");
        }
    }
}
