using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModelsApp.Dal.Migrations
{
    /// <inheritdoc />
    public partial class FixUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TargetX",
                schema: "public",
                table: "ModelInfo",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TargetY",
                schema: "public",
                table: "ModelInfo",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TargetZ",
                schema: "public",
                table: "ModelInfo",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetX",
                schema: "public",
                table: "ModelInfo");

            migrationBuilder.DropColumn(
                name: "TargetY",
                schema: "public",
                table: "ModelInfo");

            migrationBuilder.DropColumn(
                name: "TargetZ",
                schema: "public",
                table: "ModelInfo");
        }
    }
}
