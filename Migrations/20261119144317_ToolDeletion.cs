using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToolStore.Migrations
{
    /// <inheritdoc />
    public partial class ToolDeletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Tools_ToolId",
                table: "OrderItems");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Tools_ToolId",
                table: "OrderItems",
                column: "ToolId",
                principalTable: "Tools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Tools_ToolId",
                table: "OrderItems");

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ToolId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sales_Tools_ToolId",
                        column: x => x.ToolId,
                        principalTable: "Tools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_ToolId",
                table: "Sales",
                column: "ToolId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Tools_ToolId",
                table: "OrderItems",
                column: "ToolId",
                principalTable: "Tools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
