using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPL3_Banhangonline.Migrations
{
    /// <inheritdoc />
    public partial class AddRescueTablesFinalTry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Tạo bảng Chiến dịch giải cứu
            migrationBuilder.CreateTable(
                name: "RescueCampaigns",
                columns: table => new
                {
                    CampaignID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MinQuantity = table.Column<int>(type: "int", nullable: false),
                    ExpectedHarvestDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShopID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RescueCampaigns", x => x.CampaignID);
                    table.ForeignKey(
                        name: "FK_RescueCampaigns_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RescueCampaigns_Shops_ShopID",
                        column: x => x.ShopID,
                        principalTable: "Shops",
                        principalColumn: "ShopID",
                        onDelete: ReferentialAction.Cascade);
                });

            // 2. Tạo bảng Đăng ký giải cứu
            migrationBuilder.CreateTable(
                name: "RescueRegistrations",
                columns: table => new
                {
                    RegistrationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampaignID = table.Column<int>(type: "int", nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RescueRegistrations", x => x.RegistrationID);
                    table.ForeignKey(
                        name: "FK_RescueRegistrations_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.NoAction); // Chặn vòng lặp xóa
                    table.ForeignKey(
                        name: "FK_RescueRegistrations_RescueCampaigns_CampaignID",
                        column: x => x.CampaignID,
                        principalTable: "RescueCampaigns",
                        principalColumn: "CampaignID",
                        onDelete: ReferentialAction.NoAction); // Chặn vòng lặp xóa
                });

            migrationBuilder.CreateIndex(name: "IX_RescueCampaigns_CategoryID", table: "RescueCampaigns", column: "CategoryID");
            migrationBuilder.CreateIndex(name: "IX_RescueCampaigns_ShopID", table: "RescueCampaigns", column: "ShopID");
            migrationBuilder.CreateIndex(name: "IX_RescueRegistrations_CampaignID", table: "RescueRegistrations", column: "CampaignID");
            migrationBuilder.CreateIndex(name: "IX_RescueRegistrations_CustomerID", table: "RescueRegistrations", column: "CustomerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "RescueRegistrations");
            migrationBuilder.DropTable(name: "RescueCampaigns");
        }
    }
}
