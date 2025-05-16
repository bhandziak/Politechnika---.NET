using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarWorkshopProjekt.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UsedParts_PartId",
                table: "UsedParts",
                column: "PartId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsedParts_Parts_PartId",
                table: "UsedParts",
                column: "PartId",
                principalTable: "Parts",
                principalColumn: "PartId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsedParts_Parts_PartId",
                table: "UsedParts");

            migrationBuilder.DropIndex(
                name: "IX_UsedParts_PartId",
                table: "UsedParts");
        }
    }
}
