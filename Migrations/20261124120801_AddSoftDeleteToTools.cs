using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToolStore.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToTools : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Tools_ToolId",
                table: "OrderItems");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Tools",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Tools_ToolId",
                table: "OrderItems",
                column: "ToolId",
                principalTable: "Tools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Tools_ToolId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Tools");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Tools_ToolId",
                table: "OrderItems",
                column: "ToolId",
                principalTable: "Tools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
