using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarWorkshopProjekt.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationshipsServiceTaskUsedPart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UsedParts_ServiceTaskId",
                table: "UsedParts",
                column: "ServiceTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsedParts_ServiceTasks_ServiceTaskId",
                table: "UsedParts",
                column: "ServiceTaskId",
                principalTable: "ServiceTasks",
                principalColumn: "ServiceTaskId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsedParts_ServiceTasks_ServiceTaskId",
                table: "UsedParts");

            migrationBuilder.DropIndex(
                name: "IX_UsedParts_ServiceTaskId",
                table: "UsedParts");
        }
    }
}
