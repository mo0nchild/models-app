using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModelsApp.Dal.Migrations
{
    /// <inheritdoc />
    public partial class FixUpdate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Model_UserId",
                schema: "public",
                table: "Comment");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ModelId",
                schema: "public",
                table: "Comment",
                column: "ModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Model_ModelId",
                schema: "public",
                table: "Comment",
                column: "ModelId",
                principalSchema: "public",
                principalTable: "Model",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Model_ModelId",
                schema: "public",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_ModelId",
                schema: "public",
                table: "Comment");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Model_UserId",
                schema: "public",
                table: "Comment",
                column: "UserId",
                principalSchema: "public",
                principalTable: "Model",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
